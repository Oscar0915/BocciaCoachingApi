using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BocciaCoaching.Data;
using BocciaCoaching.Repositories.AssesstStrength;
using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Tests
{
    public class AssessStrengthRepositoryIntegrationTests
    {
        private ApplicationDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                // El repositorio usa transacciones; la BD en memoria no las soporta y por defecto lanza.
                .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateAssessmentIfNoneActiveAsync_NoCreaSiExisteActiva()
        {
            // Arrange
            var context = CreateInMemoryContext("testdb1");
            var repo = new AssessStrengthRepository(context);

            // Seed: evaluación A para el equipo
            var teamId = Guid.NewGuid();
            context.AssessStrengths.Add(new AssessStrength { TeamId = teamId, State = "A", EvaluationDate = System.DateTime.Now });
            await context.SaveChangesAsync();

            var dto = new AddAssessStrengthDto { TeamId = teamId, CoachId = Guid.NewGuid(), Description = "Nuevo" };

            // Act
            var response = await repo.CreateAssessmentIfNoneActiveAsync(dto);

            // Assert
            response.Success.Should().BeFalse();
            response.Message.Should().Contain("Ya existe una evaluación activa");
        }

        [Fact]
        public async Task AgregarDetalleDeEvaluacion_AlInsertarThrow36_CambiaEstadoA_T()
        {
            // Arrange
            var context = CreateInMemoryContext("testdb2");
            var repo = new AssessStrengthRepository(context);

            // Seed assessStrength
            var assess = new AssessStrength { TeamId = Guid.NewGuid(), State = "A", EvaluationDate = System.DateTime.Now };
            context.AssessStrengths.Add(assess);
            // El repositorio exige que el atleta exista en User
            var athleteId = Guid.NewGuid();
            context.Users.Add(new User { UserId = athleteId, Email = "atleta@example.com" });
            await context.SaveChangesAsync();

            var detailDto = new BocciaCoaching.Models.DTO.AssessStrength.RequestAddDetailToEvaluationForAthlete
            {
                AssessStrengthId = assess.AssessStrengthId,
                AthleteId = athleteId,
                ThrowOrder = 36,
                ScoreObtained = 3,
                Status = true,
                TargetDistance = 4
            };

            // Act
            var ok = await repo.AgregarDetalleDeEvaluacion(detailDto, false);

            // Assert
            ok.Should().BeTrue();
            var updated = await context.AssessStrengths.FindAsync(assess.AssessStrengthId);
            updated.State.Should().Be("T");
        }
    }
}

