using BocciaCoaching.Infrastructure.Auth;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Auth;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Services;
using BocciaCoaching.Services.Auth;
using BocciaCoaching.Services.Interfaces;
using BocciaCoaching.Utils;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Moq;
using Xunit;

namespace BocciaCoaching.Tests;

public class TokenServiceTests
{
    private static readonly JwtOptions Options = new() { SigningKey = new string('k', 48) };

    private static TokenService CreateService(IMemoryCache? cache = null)
        => new(Microsoft.Extensions.Options.Options.Create(Options), cache ?? new MemoryCache(new MemoryCacheOptions()));

    [Fact]
    public void Constructor_ConClaveCorta_Falla()
    {
        var act = () => new TokenService(
            Microsoft.Extensions.Options.Options.Create(new JwtOptions { SigningKey = "corta" }),
            new MemoryCache(new MemoryCacheOptions()));

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task CreateAccessToken_IncluyeUsuarioYRoles_YEsValido()
    {
        var service = CreateService();
        var userId = Guid.NewGuid();

        var (token, expiresAt) = service.CreateAccessToken(userId, "coach@example.com", new[] { AuthRoles.Coach });

        expiresAt.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(Options.AccessTokenMinutes), TimeSpan.FromSeconds(5));
        var result = await new JsonWebTokenHandler { MapInboundClaims = false }
            .ValidateTokenAsync(token, service.GetAccessTokenValidationParameters());
        result.IsValid.Should().BeTrue();
        result.ClaimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub)!.Value.Should().Be(userId.ToString());
        result.ClaimsIdentity.FindFirst("role")!.Value.Should().Be(AuthRoles.Coach);
    }

    [Fact]
    public async Task PasswordResetToken_EsDeUnSoloUso()
    {
        var service = CreateService();
        var (token, _) = service.CreatePasswordResetToken("Usuario@Example.com ");

        (await service.ConsumePasswordResetTokenAsync(token)).Should().Be("usuario@example.com");
        (await service.ConsumePasswordResetTokenAsync(token)).Should().BeNull();
    }

    [Fact]
    public async Task PasswordResetToken_NoSirveComoAccessToken_NiAlReves()
    {
        var service = CreateService();
        var (resetToken, _) = service.CreatePasswordResetToken("usuario@example.com");
        var (accessToken, _) = service.CreateAccessToken(Guid.NewGuid(), "usuario@example.com", Array.Empty<string>());

        var asAccess = await new JsonWebTokenHandler().ValidateTokenAsync(resetToken, service.GetAccessTokenValidationParameters());
        asAccess.IsValid.Should().BeFalse();
        (await service.ConsumePasswordResetTokenAsync(accessToken)).Should().BeNull();
    }

    [Fact]
    public void RefreshToken_SeGuardaSoloElHash()
    {
        var service = CreateService();
        var (token, hash) = service.CreateRefreshToken();

        hash.Should().HaveLength(64).And.NotContain(token);
        service.HashRefreshToken(token).Should().Be(hash);
    }
}

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _repository = new();
    private readonly Mock<IUserService> _users = new();
    private readonly Mock<IEmailService> _email = new();
    private readonly TokenService _tokens = new(
        Options.Create(new JwtOptions { SigningKey = new string('k', 48) }),
        new MemoryCache(new MemoryCacheOptions()));

    private AuthService CreateService() => new(
        _repository.Object, _users.Object, _email.Object, _tokens,
        new MemoryCache(new MemoryCacheOptions()),
        Options.Create(new JwtOptions { SigningKey = new string('k', 48) }),
        NullLogger<AuthService>.Instance);

    [Fact]
    public async Task Login_ConCredencialesInvalidas_Devuelve401Generico()
    {
        _users.Setup(u => u.Login(It.IsAny<LoginRequestDto>()))
            .ReturnsAsync(ResponseContract<LoginResponseDto>.Fail("Contraseña incorrecta"));

        var result = await CreateService().LoginAsync(new LoginRequestDto { Email = "a@b.com", Password = "x" }, null);

        result.Status.Should().Be(ResultStatus.Unauthorized);
        result.Message.Should().Be("Credenciales inválidas");
    }

    [Fact]
    public async Task Login_Correcto_DevuelveTokensYGuardaElRefreshToken()
    {
        var userId = Guid.NewGuid();
        _users.Setup(u => u.Login(It.IsAny<LoginRequestDto>()))
            .ReturnsAsync(ResponseContract<LoginResponseDto>.Ok(new LoginResponseDto { UserId = userId }));
        _repository.Setup(r => r.GetTokenSubjectAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TokenSubject(userId, "a@b.com", new[] { RoleIds.Coach }));

        var result = await CreateService().LoginAsync(new LoginRequestDto { Email = "a@b.com", Password = "x" }, "1.2.3.4");

        result.Success.Should().BeTrue();
        result.Data.Tokens!.AccessToken.Should().NotBeNullOrEmpty();
        _repository.Verify(r => r.AddRefreshTokenAsync(
            It.Is<RefreshToken>(t => t.UserId == userId && t.TokenHash == _tokens.HashRefreshToken(result.Data.Tokens.RefreshToken)),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Refresh_ConTokenYaRotadoHaceTiempo_RevocaTodaLaSesion()
    {
        var family = Guid.NewGuid();
        _repository.Setup(r => r.FindRefreshTokenByHashAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RefreshToken
            {
                FamilyId = family,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                RevokedAt = DateTime.UtcNow.AddMinutes(-5),
                RevokedReason = "rotated"
            });

        var result = await CreateService().RefreshAsync("robado", null);

        result.Status.Should().Be(ResultStatus.Unauthorized);
        _repository.Verify(r => r.RevokeFamilyAsync(family, "reuse-detected", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Refresh_ConTokenRotadoHaceSegundos_NoRevocaLaSesion()
    {
        _repository.Setup(r => r.FindRefreshTokenByHashAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RefreshToken
            {
                FamilyId = Guid.NewGuid(),
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                RevokedAt = DateTime.UtcNow.AddSeconds(-2),
                RevokedReason = "rotated"
            });

        var result = await CreateService().RefreshAsync("token", null);

        result.Status.Should().Be(ResultStatus.Unauthorized);
        _repository.Verify(r => r.RevokeFamilyAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ResetPassword_SinTokenValido_NoCambiaLaContrasena()
    {
        var result = await CreateService().ResetPasswordAsync(new ResetPasswordDto
        {
            Email = "victima@example.com",
            NewPassword = "nueva-clave",
            ResetToken = "inventado"
        });

        result.Status.Should().Be(ResultStatus.Unauthorized);
        _users.Verify(u => u.ResetPassword(It.IsAny<ResetPasswordDto>()), Times.Never);
    }

    [Fact]
    public async Task ResetPassword_ConTokenDeOtroEmail_NoCambiaLaContrasena()
    {
        var (token, _) = _tokens.CreatePasswordResetToken("atacante@example.com");

        var result = await CreateService().ResetPasswordAsync(new ResetPasswordDto
        {
            Email = "victima@example.com",
            NewPassword = "nueva-clave",
            ResetToken = token
        });

        result.Success.Should().BeFalse();
        _users.Verify(u => u.ResetPassword(It.IsAny<ResetPasswordDto>()), Times.Never);
    }

    [Fact]
    public async Task VerifyCode_TrasCincoIntentosFallidos_InvalidaElCodigo()
    {
        _email.Setup(e => e.ValidateCode(It.IsAny<EmailParametersDto>()))
            .Returns(new EmailValidateCodeResponseDto { StateCode = 400 });
        var service = CreateService();

        for (var i = 0; i < 5; i++)
            await service.VerifyCodeAsync("a@b.com", "000000");
        var result = await service.VerifyCodeAsync("a@b.com", "123456");

        result.Success.Should().BeFalse();
        _email.Verify(e => e.InvalidateCode("a@b.com"), Times.Once);
        _email.Verify(e => e.ValidateCode(It.IsAny<EmailParametersDto>()), Times.Exactly(5));
    }

    [Fact]
    public async Task VerifyCode_TrasPedirUnCodigoNuevo_ReiniciaLosIntentos()
    {
        _email.SetupSequence(e => e.ValidateCode(It.IsAny<EmailParametersDto>()))
            .Returns(new EmailValidateCodeResponseDto { StateCode = 400 })
            .Returns(new EmailValidateCodeResponseDto { StateCode = 400 })
            .Returns(new EmailValidateCodeResponseDto { StateCode = 400 })
            .Returns(new EmailValidateCodeResponseDto { StateCode = 400 })
            .Returns(new EmailValidateCodeResponseDto { StateCode = 400 })
            .Returns(new EmailValidateCodeResponseDto { StateCode = 200 });
        var service = CreateService();

        for (var i = 0; i < 5; i++)
            await service.VerifyCodeAsync("a@b.com", "000000");
        await service.SendVerificationCodeAsync("a@b.com");
        var result = await service.VerifyCodeAsync("a@b.com", "123456");

        result.Success.Should().BeTrue();
        result.Data.ResetToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ChangePassword_UsaElUsuarioDelToken_IgnorandoElDelCuerpo()
    {
        var tokenUser = Guid.NewGuid();
        UpdatePasswordDto? sent = null;
        _users.Setup(u => u.UpdatePassword(It.IsAny<UpdatePasswordDto>()))
            .Callback<UpdatePasswordDto>(dto => sent = dto)
            .ReturnsAsync(ResponseContract<bool>.Ok(true));

        await CreateService().ChangePasswordLegacyAsync(tokenUser, new UpdatePasswordDto
        {
            UserId = Guid.NewGuid(),
            Email = "otro@example.com",
            CurrentPassword = "actual",
            NewPassword = "nueva-clave",
            ConfirmPassword = "nueva-clave"
        });

        sent!.UserId.Should().Be(tokenUser);
        sent.Email.Should().BeNull();
        _repository.Verify(r => r.RevokeAllForUserAsync(tokenUser, "password-changed", It.IsAny<CancellationToken>()), Times.Once);
    }
}

public class UserServiceSecurityTests
{
    private readonly Mock<IUserRepository> _repository = new();

    private UserService CreateService() => new(
        _repository.Object, Mock.Of<INotificationService>(), Mock.Of<IFirebaseAuthenticationService>());

    [Fact]
    public async Task AddUser_ConRolAdmin_SeRechaza()
    {
        var result = await CreateService().AddUser(new InfoUserRegisterDto
        {
            Email = "a@b.com", Name = "A", Password = "x", Region = "R", Rol = RoleIds.Admin
        });

        result.Success.Should().BeFalse();
        _repository.Verify(r => r.AddUser(It.IsAny<InfoUserRegisterDto>()), Times.Never);
    }

    [Fact]
    public async Task UpdatePassword_SoloConEmail_SeRechaza()
    {
        var result = await CreateService().UpdatePassword(new UpdatePasswordDto
        {
            Email = "victima@example.com",
            NewPassword = "nueva-clave",
            ConfirmPassword = "nueva-clave"
        });

        result.Success.Should().BeFalse();
        _repository.Verify(r => r.UpdatePassword(It.IsAny<UpdatePasswordDto>()), Times.Never);
    }
}
