using System;
using System.Threading.Tasks;
using BocciaCoaching.Controllers;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Services;
using BocciaCoaching.Services.Interfaces;
using BocciaCoaching.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BocciaCoaching.Tests;

public class UserServiceGoogleRegistrationTests
{
    private readonly Mock<IUserRepository> _repository = new();
    private readonly Mock<IFirebaseAuthenticationService> _firebase = new();

    private UserService CreateService(string provider = "google.com", bool verified = true)
    {
        _firebase.Setup(service => service.VerifyIdTokenAsync("token"))
            .ReturnsAsync(new FirebaseIdentity("google@example.com", verified, provider));
        return new UserService(_repository.Object, Mock.Of<INotificationService>(), _firebase.Object);
    }

    [Fact]
    public async Task VerifyGoogleEmail_ReturnsVerifiedEmailWithoutAccessingRepository()
    {
        var result = await CreateService().VerifyGoogleEmail(new FirebaseLoginRequestDto { IdToken = "token" });

        result.Success.Should().BeTrue();
        result.Data.Email.Should().Be("google@example.com");
        result.Data.EmailVerified.Should().BeTrue();
        _repository.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("password", true)]
    [InlineData("facebook.com", true)]
    [InlineData(null, true)]
    [InlineData("google.com", false)]
    public async Task GoogleEndpoints_RejectWrongProviderOrUnverifiedEmail(string provider, bool verified)
    {
        var service = CreateService(provider, verified);
        var login = new FirebaseLoginRequestDto { IdToken = "token" };

        (await service.VerifyGoogleEmail(login)).Success.Should().BeFalse();
        (await service.LoginWithGoogle(login)).Success.Should().BeFalse();
        (await service.RegisterWithGoogle(new GoogleRegisterRequestDto
        {
            IdToken = "token",
            Name = "Usuario"
        })).Success.Should().BeFalse();
        _repository.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GoogleEndpoints_RejectEmptyToken(string token)
    {
        var service = CreateService();
        var login = new FirebaseLoginRequestDto { IdToken = token };

        (await service.VerifyGoogleEmail(login)).Success.Should().BeFalse();
        (await service.LoginWithGoogle(login)).Success.Should().BeFalse();
        (await service.RegisterWithGoogle(new GoogleRegisterRequestDto
        {
            IdToken = token,
            Name = "Usuario"
        })).Success.Should().BeFalse();
        _firebase.Verify(service => service.VerifyIdTokenAsync(It.IsAny<string>()), Times.Never);
        _repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RegisterWithGoogle_UsesVerifiedEmailAndRandomPassword()
    {
        var service = CreateService();
        InfoUserRegisterDto saved = null;
        _repository.Setup(repository => repository.AddUser(It.IsAny<InfoUserRegisterDto>()))
            .Callback<InfoUserRegisterDto>(user => saved = user)
            .ReturnsAsync(ResponseContract<bool>.Ok(true));
        var request = new GoogleRegisterRequestDto
        {
            IdToken = "token",
            Name = " Usuario ",
            Region = "Mexico",
            Rol = RoleIds.Athlete
        };

        var result = await service.RegisterWithGoogle(request);

        result.Success.Should().BeTrue();
        saved.Email.Should().Be("google@example.com");
        saved.Name.Should().Be("Usuario");
        saved.Region.Should().Be(request.Region);
        saved.Rol.Should().Be(RoleIds.Athlete);
        Convert.FromBase64String(saved.Password).Should().HaveCount(32);
        var firstPassword = saved.Password;
        await service.RegisterWithGoogle(request);
        saved.Password.Should().NotBe(firstPassword);
    }

    [Fact]
    public async Task RegisterWithGoogle_PropagatesDuplicateEmailFailure()
    {
        _repository.Setup(repository => repository.AddUser(It.IsAny<InfoUserRegisterDto>()))
            .ReturnsAsync(ResponseContract<bool>.Fail("Ya existe un usuario con ese email"));

        var result = await CreateService().RegisterWithGoogle(new GoogleRegisterRequestDto
        {
            IdToken = "token",
            Name = "Usuario"
        });

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Ya existe un usuario con ese email");
    }

    [Fact]
    public async Task RegisterWithGoogle_RejectsAdministrativeOrUnknownRole()
    {
        var service = CreateService();
        foreach (var role in new[] { RoleIds.Admin, Guid.Empty, Guid.NewGuid() })
        {
            var result = await service.RegisterWithGoogle(new GoogleRegisterRequestDto
            {
                IdToken = "token",
                Name = "Usuario",
                Rol = role
            });
            result.Success.Should().BeFalse();
        }
        _repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RegisterWithGoogle_RejectsEmptyName()
    {
        var result = await CreateService().RegisterWithGoogle(new GoogleRegisterRequestDto
        {
            IdToken = "token",
            Name = " "
        });

        result.Success.Should().BeFalse();
        _repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GoogleEndpoints_ReportInvalidToken()
    {
        var service = CreateService();
        _firebase.Setup(firebase => firebase.VerifyIdTokenAsync("token"))
            .ThrowsAsync(new ArgumentException("invalid token"));
        var login = new FirebaseLoginRequestDto { IdToken = "token" };

        (await service.VerifyGoogleEmail(login)).Message.Should().Contain("no es válido");
        (await service.LoginWithGoogle(login)).Success.Should().BeFalse();
        (await service.RegisterWithGoogle(new GoogleRegisterRequestDto
        {
            IdToken = "token",
            Name = "Usuario"
        })).Success.Should().BeFalse();
        _repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task VerifyGoogleEmail_ReportsMissingConfiguration()
    {
        var service = CreateService();
        _firebase.Setup(firebase => firebase.VerifyIdTokenAsync("token"))
            .ThrowsAsync(new InvalidOperationException("missing configuration"));

        var result = await service.VerifyGoogleEmail(new FirebaseLoginRequestDto { IdToken = "token" });

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("no está configurada");
    }

    [Fact]
    public async Task Controller_UsesExistingResponseContract()
    {
        var service = new Mock<IUserService>();
        var verificationRequest = new FirebaseLoginRequestDto { IdToken = "token" };
        var registrationRequest = new GoogleRegisterRequestDto { IdToken = "token", Name = "Usuario" };
        var verification = ResponseContract<GoogleEmailVerificationDto>.Ok(
            new GoogleEmailVerificationDto { Email = "google@example.com", EmailVerified = true });
        var registration = ResponseContract<bool>.Ok(true);
        service.Setup(user => user.VerifyGoogleEmail(verificationRequest)).ReturnsAsync(verification);
        service.Setup(user => user.RegisterWithGoogle(registrationRequest)).ReturnsAsync(registration);
        var controller = new UserController(service.Object, Mock.Of<IFileStorageService>(), Mock.Of<BocciaCoaching.Services.Auth.IAuthService>());

        var verifiedResult = await controller.VerifyGoogleEmail(verificationRequest);
        var registeredResult = await controller.RegisterWithGoogle(registrationRequest);

        verifiedResult.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeSameAs(verification);
        registeredResult.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeSameAs(registration);
    }
}
