using Xunit;
using Moq;
using FluentAssertions;
using BocciaCoaching.Services;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Repositories.NotificationTypes;
using BocciaCoaching.Models.DTO.Notification;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using BocciaCoaching.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Tests
{
    public class NotificationServiceTests
    {
        private static NotificationService CreateService(INotificationTypeRepository repo, IUserRepository userRepo)
            => new(repo, userRepo, Mock.Of<ITeamService>(), Mock.Of<ITeamRepository>(), Mock.Of<IEmailService>(),
                new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options));

        [Fact]
        public async Task CreateType_ConNombreNulo_ReturnsFail()
        {
            var repoMock = new Mock<INotificationTypeRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var service = CreateService(repoMock.Object, userRepoMock.Object);

            var result = await service.CreateType(new RequestCreateNotificationTypeDto { Name = null });

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("requerido");
        }

        [Fact]
        public async Task CreateMessage_SiRemitenteNoExiste_ReturnsFail()
        {
            var repoMock = new Mock<INotificationTypeRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var typeId = Guid.NewGuid();
            repoMock.Setup(r => r.GetByIdAsync(typeId)).ReturnsAsync(new NotificationType { NotificationTypeId = typeId, Name = "x" });
            userRepoMock.Setup(u => u.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(ResponseContract<InfoBasicUserDto>.Fail("Usuario no encontrado"));

            var service = CreateService(repoMock.Object, userRepoMock.Object);

            var dto = new RequestCreateNotificationMessageDto { SenderId = Guid.NewGuid(), ReceiverId = Guid.NewGuid(), NotificationTypeId = typeId };
            var res = await service.CreateMessage(dto);

            res.Success.Should().BeFalse();
            res.Message.Should().Contain("Remitente inválido");
        }
    }
}

