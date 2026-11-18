using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IWellness;
using BocciaCoaching.Services;
using BocciaCoaching.Services.Wellness;
using FluentAssertions;
using Moq;
using Xunit;

namespace BocciaCoaching.Tests;

public class WellnessReadinessCalculatorTests
{
    [Theory]
    [InlineData(5, 5, 5, 5, 5, 25, "Green")]
    [InlineData(4, 4, 4, 4, 4, 20, "Green")]
    [InlineData(4, 4, 4, 4, 3, 19, "Yellow")]
    [InlineData(3, 3, 3, 3, 3, 15, "Yellow")]
    [InlineData(3, 3, 3, 3, 2, 14, "Red")]
    [InlineData(1, 1, 1, 1, 1, 5, "Red")]
    public void Semaforo_SegunPuntajeTotal(int fatigue, int sleep, int pain, int stress, int mood, int total, string level)
    {
        var result = WellnessReadinessCalculator.Evaluate(fatigue, sleep, pain, stress, mood);

        result.TotalScore.Should().Be(total);
        result.Level.ToString().Should().Be(level);
        result.Feedback.Level.Should().Be(level);
    }

    [Fact]
    public void Veto_VerdeConDolorMuscularEn1_DegradaAAmarilloConAlerta()
    {
        // 5+5+1+5+5 = 21 (Verde por total) pero dolor muscular = 1
        var result = WellnessReadinessCalculator.Evaluate(fatigue: 5, sleep: 5, musclePain: 1, stress: 5, mood: 5);

        result.TotalScore.Should().Be(21);
        result.Level.Should().Be(ReadinessLevel.Yellow);
        result.VetoApplied.Should().BeTrue();
        result.Feedback.Title.Should().Be("Modificación Inteligente");
        result.Feedback.ItemAlerts.Should().ContainSingle(a => a.Item == "MusclePain" && a.Score == 1)
            .Which.Message.Should().Contain("dolor muscular agudo");
    }

    [Fact]
    public void Veto_VerdeConSuenoEn2_DegradaAAmarillo()
    {
        var result = WellnessReadinessCalculator.Evaluate(fatigue: 5, sleep: 2, musclePain: 5, stress: 4, mood: 5);

        result.Level.Should().Be(ReadinessLevel.Yellow);
        result.VetoApplied.Should().BeTrue();
        result.Feedback.ItemAlerts.Should().ContainSingle(a => a.Item == "Sleep");
    }

    [Fact]
    public void SinItemsCriticos_NoHayVetoNiAlertas()
    {
        var result = WellnessReadinessCalculator.Evaluate(4, 4, 4, 4, 4);

        result.VetoApplied.Should().BeFalse();
        result.Feedback.ItemAlerts.Should().BeEmpty();
    }

    [Fact]
    public void Rojo_ConItemsCriticos_NoMarcaVetoPeroIncluyeAlertas()
    {
        var result = WellnessReadinessCalculator.Evaluate(fatigue: 1, sleep: 2, musclePain: 3, stress: 2, mood: 3);

        result.Level.Should().Be(ReadinessLevel.Red);
        result.VetoApplied.Should().BeFalse();
        result.Feedback.Title.Should().Be("Recuperación Obligatoria");
        result.Feedback.ItemAlerts.Select(a => a.Item).Should().BeEquivalentTo("Fatigue", "Sleep", "Stress");
    }

    [Fact]
    public void ValoresFueraDeRango_Lanzan()
    {
        var act = () => WellnessReadinessCalculator.Evaluate(6, 5, 5, 5, 5);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void RegistroAntiguo_NoTieneFeedback()
    {
        var legacy = new ResponseDailyWellnessDto { ScaleVersion = DailyWellness.LegacyScaleVersion, Fatigue = 7, Sleep = 6, MusclePain = 5, Stress = 4 };
        WellnessReadinessCalculator.FeedbackFor(legacy).Should().BeNull();
    }
}

public class WellnessServiceTests
{
    private static readonly TimeZoneInfo Bogota = WellnessRequester.ResolveTimeZone("America/Bogota", "UTC");
    private readonly Mock<IWellnessRepository> _repository = new();

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }

    // 19-nov-2026 01:30 UTC = 18-nov-2026 20:30 en Bogotá
    private static readonly DateTimeOffset EveningInBogota = new(2026, 11, 19, 1, 30, 0, TimeSpan.Zero);

    private WellnessService CreateService(DateTimeOffset? now = null)
        => new(_repository.Object, new FixedTimeProvider(now ?? EveningInBogota));

    private static WellnessRequester Athlete(Guid id) => new(id, IsAdmin: false, IsCoach: false, IsAthlete: true, Bogota);
    private static WellnessRequester Coach(Guid id) => new(id, IsAdmin: false, IsCoach: true, IsAthlete: false, Bogota);

    private static SubmitMyWellnessDto Answers(int value = 4)
        => new() { Fatigue = value, Sleep = value, MusclePain = value, Stress = value, Mood = value };

    [Fact]
    public async Task EstadoDeHoy_SinRegistro_DebeCompletarlo_UsandoElDiaLocal()
    {
        var athleteId = Guid.NewGuid();

        var result = await CreateService().GetMyTodayStatusAsync(Athlete(athleteId));

        result.Data.Date.Should().Be(new DateTime(2026, 11, 18)); // no el 19 (UTC)
        result.Data.Required.Should().BeTrue();
        result.Data.Completed.Should().BeFalse();
        result.Data.MustComplete.Should().BeTrue();
        _repository.Verify(r => r.GetByAthleteAndDateAsync(athleteId, new DateTime(2026, 11, 18), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task EstadoDeHoy_ConRegistro_DevuelveResultadoConFeedback()
    {
        var athleteId = Guid.NewGuid();
        _repository.Setup(r => r.GetByAthleteAndDateAsync(athleteId, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResponseDailyWellnessDto
            {
                AthleteId = athleteId, ScaleVersion = 2, Fatigue = 3, Sleep = 3, MusclePain = 3, Stress = 3, Mood = 3,
                TotalScore = 15, ReadinessLevel = "Yellow"
            });

        var result = await CreateService().GetMyTodayStatusAsync(Athlete(athleteId));

        result.Data.MustComplete.Should().BeFalse();
        result.Data.Assessment!.Feedback!.Level.Should().Be("Yellow");
    }

    [Fact]
    public async Task EstadoDeHoy_UsuarioNoAtleta_NoEsObligatorio()
    {
        var result = await CreateService().GetMyTodayStatusAsync(Coach(Guid.NewGuid()));

        result.Data.Required.Should().BeFalse();
        result.Data.MustComplete.Should().BeFalse();
    }

    [Fact]
    public async Task RegistrarPropio_CalculaSemaforoYGuardaElDiaLocal()
    {
        var athleteId = Guid.NewGuid();
        DailyWellness? saved = null;
        _repository.Setup(r => r.AthleteExistsAsync(athleteId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _repository.Setup(r => r.AddAsync(It.IsAny<DailyWellness>(), It.IsAny<CancellationToken>()))
            .Callback<DailyWellness, CancellationToken>((e, _) => saved = e);
        _repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new ResponseDailyWellnessDto
            {
                AthleteId = athleteId, ScaleVersion = saved!.ScaleVersion, Fatigue = saved.Fatigue, Sleep = saved.Sleep,
                MusclePain = saved.MusclePain, Stress = saved.Stress, Mood = saved.Mood,
                TotalScore = saved.TotalScore, ReadinessLevel = saved.ReadinessLevel, VetoApplied = saved.VetoApplied
            });

        var dto = new SubmitMyWellnessDto { Fatigue = 5, Sleep = 5, MusclePain = 1, Stress = 5, Mood = 5 };
        var result = await CreateService().SubmitMyWellnessAsync(Athlete(athleteId), dto);

        result.Status.Should().Be(ResultStatus.Created);
        saved!.AssessmentDate.Should().Be(new DateTime(2026, 11, 18));
        saved.ScaleVersion.Should().Be(DailyWellness.McLeanScaleVersion);
        saved.TotalScore.Should().Be(21);
        saved.ReadinessLevel.Should().Be("Yellow");
        saved.VetoApplied.Should().BeTrue();
        result.Data.Feedback!.ItemAlerts.Should().ContainSingle(a => a.Item == "MusclePain");
    }

    [Fact]
    public async Task RegistrarPropio_SiYaLoHizoHoy_Devuelve409()
    {
        var athleteId = Guid.NewGuid();
        _repository.Setup(r => r.ExistsForDayAsync(athleteId, new DateTime(2026, 11, 18), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await CreateService().SubmitMyWellnessAsync(Athlete(athleteId), Answers());

        result.Status.Should().Be(ResultStatus.Conflict);
        _repository.Verify(r => r.AddAsync(It.IsAny<DailyWellness>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RegistrarPropio_SiNoEsAtleta_Devuelve403()
    {
        var result = await CreateService().SubmitMyWellnessAsync(Coach(Guid.NewGuid()), Answers());
        result.Status.Should().Be(ResultStatus.Forbidden);
    }

    [Fact]
    public async Task Registrar_ParaOtroAtletaSinSerSuEntrenador_Devuelve403()
    {
        var dto = new AddDailyWellnessDto { AthleteId = Guid.NewGuid(), Fatigue = 4, Sleep = 4, MusclePain = 4, Stress = 4, Mood = 4 };

        var result = await CreateService().RegisterDailyWellnessAsync(Athlete(Guid.NewGuid()), dto);

        result.Status.Should().Be(ResultStatus.Forbidden);
    }

    [Fact]
    public async Task Registrar_AtletaNoPuedeRegistrarOtroDia()
    {
        var athleteId = Guid.NewGuid();
        var dto = new AddDailyWellnessDto
        {
            AthleteId = athleteId, AssessmentDate = new DateTime(2026, 11, 10),
            Fatigue = 4, Sleep = 4, MusclePain = 4, Stress = 4, Mood = 4
        };

        var result = await CreateService().RegisterDailyWellnessAsync(Athlete(athleteId), dto);

        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public async Task Leer_HistorialDeOtroAtleta_SinSerSuEntrenador_Devuelve403()
    {
        var result = await CreateService().GetAthleteHistoryAsync(Coach(Guid.NewGuid()), Guid.NewGuid());
        result.Status.Should().Be(ResultStatus.Forbidden);
    }

    [Fact]
    public async Task ResumenDeEquipo_OrdenaRojosPrimeroYListaPendientes()
    {
        var coachId = Guid.NewGuid();
        var teamId = Guid.NewGuid();
        var red = Guid.NewGuid(); var green = Guid.NewGuid(); var pending = Guid.NewGuid();
        _repository.Setup(r => r.TeamExistsAsync(teamId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _repository.Setup(r => r.IsCoachOfTeamAsync(coachId, teamId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _repository.Setup(r => r.GetTeamAthletesAsync(teamId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<WellnessPendingAthleteDto>
        {
            new() { AthleteId = red, AthleteName = "Rojo" }, new() { AthleteId = green, AthleteName = "Verde" },
            new() { AthleteId = pending, AthleteName = "Pendiente" }
        });
        _repository.Setup(r => r.GetTeamWellnessByDateAsync(teamId, new DateTime(2026, 11, 18), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ResponseDailyWellnessDto>
            {
                new() { AthleteId = green, ScaleVersion = 2, Fatigue = 5, Sleep = 5, MusclePain = 5, Stress = 5, Mood = 5, TotalScore = 25, ReadinessLevel = "Green" },
                new() { AthleteId = red, ScaleVersion = 2, Fatigue = 2, Sleep = 2, MusclePain = 2, Stress = 2, Mood = 2, TotalScore = 10, ReadinessLevel = "Red" }
            });

        var result = await CreateService().GetTeamSummaryAsync(Coach(coachId), teamId, null);

        result.Data.TotalAthletes.Should().Be(3);
        result.Data.Completed.Should().Be(2);
        result.Data.Red.Should().Be(1);
        result.Data.Green.Should().Be(1);
        result.Data.Assessments.First().AthleteId.Should().Be(red);
        result.Data.PendingAthletes.Should().ContainSingle(a => a.AthleteId == pending);
    }

    [Fact]
    public async Task ResumenDeEquipo_SiNoEsSuEntrenador_Devuelve403()
    {
        var teamId = Guid.NewGuid();
        _repository.Setup(r => r.TeamExistsAsync(teamId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await CreateService().GetTeamSummaryAsync(Coach(Guid.NewGuid()), teamId, null);

        result.Status.Should().Be(ResultStatus.Forbidden);
    }

    [Fact]
    public void ZonaHoraria_CabeceraInvalida_UsaLaConfigurada()
    {
        WellnessRequester.ResolveTimeZone("Zona/Inventada", "America/Bogota").BaseUtcOffset.Should().Be(TimeSpan.FromHours(-5));
        WellnessRequester.ResolveTimeZone("America/Mexico_City", "America/Bogota").Id.Should().Be("America/Mexico_City");
    }
}
