using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Services;
using BocciaCoaching.Services.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace BocciaCoaching.Tests;

public class UserServiceFirebaseLoginTests
{
    [Fact]
    public async Task LoginWithGoogle_ConEmailNoVerificado_RechazaElInicioDeSesion()
    {
        var repository = new Mock<IUserRepository>();
        var firebase = new Mock<IFirebaseAuthenticationService>();
        firebase.Setup(service => service.VerifyIdTokenAsync("token"))
            .ReturnsAsync(new FirebaseIdentity("usuario@example.com", false));
        var service = new UserService(
            repository.Object,
            Mock.Of<INotificationService>(),
            firebase.Object);

        var result = await service.LoginWithGoogle(new FirebaseLoginRequestDto { IdToken = "token" });

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("verificado");
        repository.Verify(
            repository => repository.LoginWithEmailAsync(It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task LoginWithGoogle_ConEmailVerificado_UsaElUsuarioLocal()
    {
        var repository = new Mock<IUserRepository>();
        var firebase = new Mock<IFirebaseAuthenticationService>();
        firebase.Setup(service => service.VerifyIdTokenAsync("token"))
            .ReturnsAsync(new FirebaseIdentity("usuario@example.com", true));
        repository.Setup(repository => repository.LoginWithEmailAsync("usuario@example.com"))
            .ReturnsAsync(ResponseContract<LoginResponseDto>.Ok(
                new LoginResponseDto { UserId = Guid.NewGuid() }));
        var service = new UserService(
            repository.Object,
            Mock.Of<INotificationService>(),
            firebase.Object);

        var result = await service.LoginWithGoogle(new FirebaseLoginRequestDto { IdToken = "token" });

        result.Success.Should().BeTrue();
        repository.Verify(
            repository => repository.LoginWithEmailAsync("usuario@example.com"),
            Times.Once);
    }
}
