namespace BocciaCoaching.Models.DTO.Statistic
{
    /// <summary>
    /// Estadísticas SAREMAS+ por equipo
    /// </summary>
    public class SaremasTeamStatsDto
    {
        public int TeamId { get; set; }
        public string? TeamName { get; set; }
        public int TotalEvaluations { get; set; }
        public int CompletedEvaluations { get; set; }
        public double AverageTeamScore { get; set; }
        public List<SaremasAthleteStatsItemDto> AthleteStats { get; set; } = new();
    }

    public class SaremasAthleteStatsItemDto
    {
        public int AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public int EvaluationsCompleted { get; set; }
        public double AverageScore { get; set; }
        public int? BestScore { get; set; }
        public DateTime? LastEvaluationDate { get; set; }
    }

    /// <summary>
    /// Evolución SAREMAS+ de un atleta a lo largo del tiempo
    /// </summary>
    public class SaremasAthleteEvolutionDto
    {
        public int AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public List<SaremasEvolutionPointDto> EvolutionPoints { get; set; } = new();
        public double OverallAverage { get; set; }
        public double Trend { get; set; } // Positive = improving, Negative = declining
    }

    public class SaremasEvolutionPointDto
    {
        public int EvaluationId { get; set; }
        public DateTime EvaluationDate { get; set; }
        public int TotalScore { get; set; }
        public double AverageScore { get; set; }
    }

    /// <summary>
    /// Progreso de un macrociclo
    /// </summary>
    public class MacrocycleProgressDto
    {
        public string MacrocycleId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string AthleteName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalWeeks { get; set; }
        public int CompletedWeeks { get; set; }
        public double ProgressPercentage { get; set; }
        public string? CurrentPeriod { get; set; }
        public string? CurrentMesocycle { get; set; }
        public int CurrentWeekNumber { get; set; }
        public int TotalEvents { get; set; }
        public int CompletedEvents { get; set; }
        public int UpcomingEvents { get; set; }
    }

    /// <summary>
    /// Dashboard unificado: Fuerza + Dirección + SAREMAS+ + Macrociclo
    /// </summary>
    public class AthleteFullDashboardDto
    {
        public int AthleteId { get; set; }
        public string? AthleteName { get; set; }

        // Fuerza
        public int StrengthEvaluationsCompleted { get; set; }
        public double? StrengthLastScore { get; set; }
        public double? StrengthAverageEffectiveness { get; set; }

        // Dirección
        public int DirectionEvaluationsCompleted { get; set; }
        public double? DirectionLastScore { get; set; }

        // SAREMAS+
        public int SaremasEvaluationsCompleted { get; set; }
        public int? SaremasLastTotalScore { get; set; }
        public double? SaremasAverageScore { get; set; }

        // Macrociclo activo
        public string? ActiveMacrocycleName { get; set; }
        public double? MacrocycleProgressPercentage { get; set; }
        public string? CurrentPeriod { get; set; }
    }
}

