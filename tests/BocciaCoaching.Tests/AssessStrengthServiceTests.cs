using System.Threading.Tasks;
using Moq;
using Xunit;
using FluentAssertions;
using BocciaCoaching.Services;
using BocciaCoaching.Repositories.Interfaces.IAssesstStrength;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Tests
{
    public class AssessStrengthServiceTests
    {
        [Fact]
        public async Task CreateEvaluation_CreaCuandoNoHayActiva_ReturnsOk()
        {
            // Arrange
            var repoMock = new Mock<IAssessStrengthRepository>();
            var teamValidationMock = new Mock<ITeamValidationRepository>();
            var validationsMock = new Mock<IValidationsAssetsStrength>();

            var addDto = new AddAssessStrengthDto { Description = "x", TeamId = Guid.NewGuid(), CoachId = Guid.NewGuid() };

            teamValidationMock.Setup(t => t.ValidateTeam(It.IsAny<BocciaCoaching.Models.Entities.Team>()))
                .ReturnsAsync(true);

            var assessId = Guid.NewGuid();
            var repoResponse = ResponseContract<ResponseAddAssessStrengthDto>.Ok(
                new ResponseAddAssessStrengthDto { AssessStrengthId = assessId, DateEvaluation = System.DateTime.Now, State = true },
                "ok");

            repoMock.Setup(r => r.CreateAssessmentIfNoneActiveAsync(It.IsAny<AddAssessStrengthDto>()))
                .ReturnsAsync(repoResponse);

            var service = new AssessStrengthService(repoMock.Object, teamValidationMock.Object, validationsMock.Object, Mock.Of<INotificationService>());

            // Act
            var result = await service.CreateEvaluation(addDto);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.AssessStrengthId.Should().Be(assessId);
            repoMock.Verify(r => r.CreateAssessmentIfNoneActiveAsync(It.IsAny<AddAssessStrengthDto>()), Times.Once);
        }
    }
}

