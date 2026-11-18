using BocciaCoaching.Infrastructure.Auth;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Auth;
using BocciaCoaching.Services.Interfaces;
using BocciaCoaching.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BocciaCoaching.Services.Auth
{
    public interface IAuthService
    {
        /// <summary>ES: Login con email y contraseña. Devuelve el usuario con sus tokens.</summary>
        Task<ResponseContract<LoginResponseDto>> LoginAsync(LoginRequestDto request, string? ipAddress, CancellationToken cancellationToken = default);

        /// <summary>ES: Login con un ID token de Firebase (Google). Devuelve el usuario con sus tokens.</summary>
        Task<ResponseContract<LoginResponseDto>> LoginWithGoogleAsync(FirebaseLoginRequestDto request, string? ipAddress, CancellationToken cancellationToken = default);

        /// <summary>ES: Rota el refresh token y emite un nuevo par de tokens.</summary>
        Task<ResponseContract<AuthTokensDto>> RefreshAsync(string refreshToken, string? ipAddress, CancellationToken cancellationToken = default);

        /// <summary>ES: Cierra la sesión asociada al refresh token.</summary>
        Task<ResponseContract<bool>> LogoutAsync(string refreshToken, CancellationToken cancellationToken = default);

        /// <summary>ES: Cierra todas las sesiones del usuario.</summary>
        Task<ResponseContract<bool>> LogoutAllAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>ES: Envía un código de restablecimiento si el email existe (respuesta siempre genérica).</summary>
        Task<ResponseContract<bool>> RequestPasswordResetAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>ES: Genera y envía un código de verificación al email; reinicia el contador de intentos.</summary>
        Task SendVerificationCodeAsync(string email);

        /// <summary>ES: Valida el código del correo (máx. 5 intentos) y devuelve un token de restablecimiento.</summary>
        Task<ResponseContract<PasswordResetTokenDto>> VerifyCodeAsync(string email, string code);

        /// <summary>ES: Restablece la contraseña con un token de restablecimiento y cierra todas las sesiones.</summary>
        Task<ResponseContract<bool>> ResetPasswordAsync(ResetPasswordDto request, CancellationToken cancellationToken = default);

        /// <summary>ES: Cambia la contraseña del usuario autenticado, cierra sus sesiones y emite tokens nuevos.</summary>
        Task<ResponseContract<AuthTokensDto>> ChangePasswordAsync(Guid userId, ChangePasswordDto request, string? ipAddress, CancellationToken cancellationToken = default);

        /// <summary>ES: Cambia la contraseña (contrato legacy). El usuario sale del token, nunca del cuerpo.</summary>
        Task<ResponseContract<bool>> ChangePasswordLegacyAsync(Guid userId, UpdatePasswordDto request, CancellationToken cancellationToken = default);
    }

    public sealed class AuthService(
        IAuthRepository authRepository,
        IUserService userService,
        IEmailService emailService,
        ITokenService tokenService,
        IMemoryCache cache,
        IOptions<JwtOptions> jwtOptions,
        ILogger<AuthService> logger) : IAuthService
    {
        private const int MaxCodeAttempts = 5;
        private const string InvalidCredentialsMessage = "Credenciales inválidas";
        private const string InvalidSessionMessage = "La sesión no es válida o expiró. Inicia sesión de nuevo.";
        private readonly JwtOptions _jwt = jwtOptions.Value;

        public async Task<ResponseContract<LoginResponseDto>> LoginAsync(LoginRequestDto request, string? ipAddress, CancellationToken cancellationToken = default)
        {
            var result = await userService.Login(request);
            if (!result.Success || result.Data is null)
            {
                logger.LogWarning("Login fallido desde {Ip}", ipAddress);
                return ResponseContract<LoginResponseDto>.Unauthorized(InvalidCredentialsMessage);
            }

            return await AttachTokensAsync(result.Data, ipAddress, cancellationToken);
        }

        public async Task<ResponseContract<LoginResponseDto>> LoginWithGoogleAsync(FirebaseLoginRequestDto request, string? ipAddress, CancellationToken cancellationToken = default)
        {
            var result = await userService.LoginWithGoogle(request);
            if (!result.Success || result.Data is null)
                // Se conserva el mensaje (p. ej. "Usuario no encontrado") porque el frontend lo usa
                // para ofrecer el registro con Google; el token de Google ya prueba la identidad.
                return ResponseContract<LoginResponseDto>.Unauthorized(result.Message);

            return await AttachTokensAsync(result.Data, ipAddress, cancellationToken);
        }

        public async Task<ResponseContract<AuthTokensDto>> RefreshAsync(string refreshToken, string? ipAddress, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return ResponseContract<AuthTokensDto>.Unauthorized(InvalidSessionMessage);

            var stored = await authRepository.FindRefreshTokenByHashAsync(tokenService.HashRefreshToken(refreshToken), cancellationToken);
            if (stored is null)
                return ResponseContract<AuthTokensDto>.Unauthorized(InvalidSessionMessage);

            var now = DateTime.UtcNow;

            if (stored.RevokedAt is not null)
            {
                var isRecentRotation = stored.RevokedReason == "rotated" &&
                                       now - stored.RevokedAt.Value <= TimeSpan.FromSeconds(_jwt.RefreshReuseGraceSeconds);
                if (!isRecentRotation)
                {
                    // Reuso de un token ya rotado: alguien más lo tiene. Se corta toda la sesión.
                    logger.LogWarning("Reuso de refresh token detectado. Usuario {UserId}, sesión {FamilyId}, IP {Ip}",
                        stored.UserId, stored.FamilyId, ipAddress);
                    await authRepository.RevokeFamilyAsync(stored.FamilyId, "reuse-detected", cancellationToken);
                }
                return ResponseContract<AuthTokensDto>.Unauthorized(InvalidSessionMessage);
            }

            if (stored.ExpiresAt <= now)
                return ResponseContract<AuthTokensDto>.Unauthorized(InvalidSessionMessage);

            var subject = await authRepository.GetTokenSubjectAsync(stored.UserId, cancellationToken);
            if (subject is null)
            {
                await authRepository.RevokeFamilyAsync(stored.FamilyId, "user-missing", cancellationToken);
                return ResponseContract<AuthTokensDto>.Unauthorized(InvalidSessionMessage);
            }

            var (newRefreshToken, newHash) = tokenService.CreateRefreshToken();
            var replacement = NewRefreshTokenEntity(stored.UserId, stored.FamilyId, newHash, ipAddress);

            if (!await authRepository.RotateRefreshTokenAsync(stored, replacement, cancellationToken))
                // Otra petición simultánea ya rotó este token (p. ej. dos pestañas): no es un robo.
                return ResponseContract<AuthTokensDto>.Unauthorized(InvalidSessionMessage);

            var tokens = BuildTokens(subject, newRefreshToken, replacement.ExpiresAt);
            return ResponseContract<AuthTokensDto>.Ok(tokens, "Sesión renovada");
        }

        public async Task<ResponseContract<bool>> LogoutAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var stored = await authRepository.FindRefreshTokenByHashAsync(tokenService.HashRefreshToken(refreshToken), cancellationToken);
                if (stored is not null)
                    await authRepository.RevokeFamilyAsync(stored.FamilyId, "logout", cancellationToken);
            }

            // Respuesta idéntica exista o no el token: no revela nada.
            return ResponseContract<bool>.Ok(true, "Sesión cerrada");
        }

        public async Task<ResponseContract<bool>> LogoutAllAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            await authRepository.RevokeAllForUserAsync(userId, "logout-all", cancellationToken);
            return ResponseContract<bool>.Ok(true, "Se cerraron todas las sesiones");
        }

        public async Task<ResponseContract<bool>> RequestPasswordResetAsync(string email, CancellationToken cancellationToken = default)
        {
            var normalized = email.Trim();
            if (await authRepository.GetUserIdByEmailAsync(normalized, cancellationToken) is not null)
                await SendVerificationCodeAsync(normalized);

            return ResponseContract<bool>.Ok(true, "Si el correo está registrado, recibirás un código de verificación.");
        }

        public async Task SendVerificationCodeAsync(string email)
        {
            var parameters = new EmailParametersDto { ToEmail = email.Trim(), Code = SecurityCodeGenerator.GenerateCode() };
            emailService.SaveCode(parameters);
            cache.Remove(AttemptsKey(parameters.ToEmail)); // código nuevo = intentos desde cero
            await emailService.SendSecurityCodeAsync(parameters);
        }

        public Task<ResponseContract<PasswordResetTokenDto>> VerifyCodeAsync(string email, string code)
        {
            var normalized = email?.Trim() ?? string.Empty;
            var attemptsKey = AttemptsKey(normalized);
            var attempts = cache.Get<int?>(attemptsKey) ?? 0;

            if (attempts >= MaxCodeAttempts)
            {
                emailService.InvalidateCode(normalized);
                return Task.FromResult(ResponseContract<PasswordResetTokenDto>.Invalid(
                    "Demasiados intentos. Solicita un código nuevo."));
            }

            var validation = emailService.ValidateCode(new EmailParametersDto { ToEmail = normalized, Code = code });
            if (validation.StateCode != StatusCodes.Status200OK)
            {
                cache.Set(attemptsKey, attempts + 1, TimeSpan.FromMinutes(15));
                return Task.FromResult(ResponseContract<PasswordResetTokenDto>.Invalid("Código no válido"));
            }

            cache.Remove(attemptsKey);
            var (token, expiresAt) = tokenService.CreatePasswordResetToken(normalized);
            return Task.FromResult(ResponseContract<PasswordResetTokenDto>.Ok(
                new PasswordResetTokenDto { ResetToken = token, ExpiresAt = expiresAt }, "Código válido"));
        }

        public async Task<ResponseContract<bool>> ResetPasswordAsync(ResetPasswordDto request, CancellationToken cancellationToken = default)
        {
            var authorizedEmail = await tokenService.ConsumePasswordResetTokenAsync(request.ResetToken);
            if (authorizedEmail is null ||
                !string.Equals(authorizedEmail, request.Email?.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return ResponseContract<bool>.Unauthorized(
                    "El enlace de restablecimiento no es válido o expiró. Solicita un código nuevo.");
            }

            var result = await userService.ResetPassword(request);
            if (!result.Success)
                return result;

            var userId = await authRepository.GetUserIdByEmailAsync(request.Email!, cancellationToken);
            if (userId is not null)
                await authRepository.RevokeAllForUserAsync(userId.Value, "password-reset", cancellationToken);

            return result;
        }

        public async Task<ResponseContract<AuthTokensDto>> ChangePasswordAsync(Guid userId, ChangePasswordDto request, string? ipAddress, CancellationToken cancellationToken = default)
        {
            var result = await ChangePasswordLegacyAsync(userId, new UpdatePasswordDto
            {
                UserId = userId,
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword,
                ConfirmPassword = request.ConfirmPassword
            }, cancellationToken);

            if (!result.Success)
                return new ResponseContract<AuthTokensDto>(false, result.Message, null!) { Status = result.Status };

            // Las demás sesiones quedaron cerradas; esta continúa con tokens nuevos.
            var subject = await authRepository.GetTokenSubjectAsync(userId, cancellationToken);
            var tokens = await IssueTokensAsync(subject!, ipAddress, cancellationToken);
            return ResponseContract<AuthTokensDto>.Ok(tokens, "Contraseña actualizada");
        }

        public async Task<ResponseContract<bool>> ChangePasswordLegacyAsync(Guid userId, UpdatePasswordDto request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.CurrentPassword))
                return ResponseContract<bool>.Invalid("La contraseña actual es requerida.");

            // El usuario sale SIEMPRE del token. Se ignora cualquier UserId/Email enviado en el cuerpo.
            request.UserId = userId;
            request.Email = null;

            var result = await userService.UpdatePassword(request);
            if (result.Success)
                await authRepository.RevokeAllForUserAsync(userId, "password-changed", cancellationToken);

            return result;
        }

        // ---------------- helpers ----------------

        private async Task<ResponseContract<LoginResponseDto>> AttachTokensAsync(LoginResponseDto user, string? ipAddress, CancellationToken cancellationToken)
        {
            var subject = await authRepository.GetTokenSubjectAsync(user.UserId, cancellationToken);
            if (subject is null)
                return ResponseContract<LoginResponseDto>.Unauthorized(InvalidCredentialsMessage);

            user.Tokens = await IssueTokensAsync(subject, ipAddress, cancellationToken);
            return ResponseContract<LoginResponseDto>.Ok(user, "Inicio de sesión exitoso");
        }

        private async Task<AuthTokensDto> IssueTokensAsync(TokenSubject subject, string? ipAddress, CancellationToken cancellationToken)
        {
            await authRepository.DeleteExpiredAsync(subject.UserId, TimeSpan.FromDays(7), cancellationToken);

            var (refreshToken, hash) = tokenService.CreateRefreshToken();
            var entity = NewRefreshTokenEntity(subject.UserId, Guid.NewGuid(), hash, ipAddress);
            await authRepository.AddRefreshTokenAsync(entity, cancellationToken);

            return BuildTokens(subject, refreshToken, entity.ExpiresAt);
        }

        private AuthTokensDto BuildTokens(TokenSubject subject, string refreshToken, DateTime refreshExpiresAt)
        {
            var roles = subject.RoleIds.Select(AuthRoles.FromRoleId).OfType<string>();
            var (accessToken, accessExpiresAt) = tokenService.CreateAccessToken(subject.UserId, subject.Email, roles);

            return new AuthTokensDto
            {
                AccessToken = accessToken,
                AccessTokenExpiresAt = accessExpiresAt,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshExpiresAt
            };
        }

        private RefreshToken NewRefreshTokenEntity(Guid userId, Guid familyId, string hash, string? ipAddress)
        {
            var now = DateTime.UtcNow;
            return new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                UserId = userId,
                FamilyId = familyId,
                TokenHash = hash,
                CreatedAt = now,
                ExpiresAt = now.AddDays(_jwt.RefreshTokenDays),
                CreatedByIp = ipAddress is { Length: > 45 } ? ipAddress[..45] : ipAddress
            };
        }

        private static string AttemptsKey(string email) => $"code-attempts:{email.ToLowerInvariant()}";
    }
}
