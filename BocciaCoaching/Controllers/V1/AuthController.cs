using BocciaCoaching.Infrastructure.Api;
using BocciaCoaching.Infrastructure.Auth;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Services.Auth;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BocciaCoaching.Controllers.V1
{
    /// <summary>
    /// ES: Autenticación (API REST v1): login, renovación de sesión, logout y contraseñas.
    /// </summary>
    [Route("api/v1/auth")]
    [Tags("Auth")]
    public sealed class AuthController(IAuthService authService, IUserService userService) : ApiControllerBase
    {
        private string? ClientIp => HttpContext.Connection.RemoteIpAddress?.ToString();

        /// <summary>Inicia sesión con email y contraseña. Devuelve el usuario y sus tokens.</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<ResponseContract<LoginResponseDto>>> Login(LoginRequestDto request, CancellationToken cancellationToken)
            => FromResult(await authService.LoginAsync(request, ClientIp, cancellationToken));

        /// <summary>Inicia sesión con un ID token de Firebase (Google).</summary>
        [HttpPost("google")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseContract<LoginResponseDto>>> LoginWithGoogle(FirebaseLoginRequestDto request, CancellationToken cancellationToken)
            => FromResult(await authService.LoginWithGoogleAsync(request, ClientIp, cancellationToken));

        /// <summary>
        /// Renueva la sesión. Envía el último refresh token; recibes uno nuevo (el anterior deja de servir).
        /// Reusar un refresh token ya usado cierra la sesión completa por seguridad.
        /// </summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseContract<AuthTokensDto>>> Refresh(RefreshTokenRequestDto request, CancellationToken cancellationToken)
            => FromResult(await authService.RefreshAsync(request.RefreshToken, ClientIp, cancellationToken));

        /// <summary>Cierra la sesión del refresh token indicado. Funciona aunque el access token haya expirado.</summary>
        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseContract<bool>>> Logout(RefreshTokenRequestDto request, CancellationToken cancellationToken)
            => FromResult(await authService.LogoutAsync(request.RefreshToken, cancellationToken));

        /// <summary>Cierra todas las sesiones del usuario autenticado (todos los dispositivos).</summary>
        [HttpPost("logout-all")]
        public async Task<ActionResult<ResponseContract<bool>>> LogoutAll(CancellationToken cancellationToken)
            => FromResult(await authService.LogoutAllAsync(User.GetUserId(), cancellationToken));

        /// <summary>Datos del usuario autenticado.</summary>
        [HttpGet("me")]
        public async Task<ActionResult<ResponseContract<InfoBasicUserDto>>> Me()
            => FromResult(await userService.GetByIdAsync(User.GetUserId()));

        /// <summary>
        /// Cambia la contraseña del usuario autenticado. Cierra las demás sesiones y devuelve tokens nuevos
        /// para esta.
        /// </summary>
        [HttpPost("change-password")]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        public async Task<ActionResult<ResponseContract<AuthTokensDto>>> ChangePassword(ChangePasswordDto request, CancellationToken cancellationToken)
            => FromResult(await authService.ChangePasswordAsync(User.GetUserId(), request, ClientIp, cancellationToken));

        /// <summary>Paso 1 del restablecimiento: envía un código al correo (respuesta genérica, exista o no).</summary>
        [HttpPost("password-reset/request")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        public async Task<ActionResult<ResponseContract<bool>>> RequestPasswordReset(PasswordResetRequestDto request, CancellationToken cancellationToken)
            => FromResult(await authService.RequestPasswordResetAsync(request.Email, cancellationToken));

        /// <summary>Paso 2: valida el código (máx. 5 intentos) y devuelve un token de restablecimiento de un solo uso.</summary>
        [HttpPost("password-reset/verify")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        public async Task<ActionResult<ResponseContract<PasswordResetTokenDto>>> VerifyPasswordResetCode(PasswordResetVerifyDto request)
            => FromResult(await authService.VerifyCodeAsync(request.Email, request.Code));

        /// <summary>Paso 3: fija la nueva contraseña con el token del paso 2. Cierra todas las sesiones.</summary>
        [HttpPost("password-reset/confirm")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseContract<bool>>> ConfirmPasswordReset(ResetPasswordDto request, CancellationToken cancellationToken)
            => FromResult(await authService.ResetPasswordAsync(request, cancellationToken));
    }
}
