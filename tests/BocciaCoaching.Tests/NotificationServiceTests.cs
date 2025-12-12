using Xunit;
using Moq;
using FluentAssertions;
using BocciaCoaching.Services;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Repositories.NotificationTypes;
using BocciaCoaching.Models.DTO.Notification;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Tests
{
    public class NotificationServiceTests
    {
        [Fact]
        public async Task CreateType_ConNombreNulo_ReturnsFail()
        {
            var repoMock = new Mock<INotificationTypeRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var service = new NotificationService(repoMock.Object, userRepoMock.Object);

            var result = await service.CreateType(new RequestCreateNotificationTypeDto { Name = null });

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("requerido");
        }

        [Fact]
        public async Task CreateMessage_SiCoachNoExiste_ReturnsFail()
        {
            var repoMock = new Mock<INotificationTypeRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new NotificationType { NotificationTypeId = 1, Name = "x" });
            userRepoMock.Setup(u => u.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((BocciaCoaching.Models.DTO.User.InfoBasicUserDto?)null);

            var service = new NotificationService(repoMock.Object, userRepoMock.Object);

            var dto = new RequestCreateNotificationMessageDto { CoachId = 99, AthleteId = 1, NotificationTypeId = 1 };
            var res = await service.CreateMessage(dto);

            res.Success.Should().BeFalse();
            res.Message.Should().Contain("Coach inválido");
        }
    }
}

