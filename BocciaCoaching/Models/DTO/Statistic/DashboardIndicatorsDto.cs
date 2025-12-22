namespace BocciaCoaching.Models.DTO.Statistic;

/// <summary>
/// DTO para los indicadores principales del dashboard
/// </summary>
public class DashboardIndicatorsDto
{
    /// <summary>
    /// Total de atletas en el sistema
    /// </summary>
    public int TotalAthletes { get; set; }
    
    /// <summary>
    /// Atletas activos (con evaluaciones recientes)
    /// </summary>
    public int ActiveAthletes { get; set; }
    
    /// <summary>
    /// Total de pruebas/evaluaciones realizadas
    /// </summary>
    public int CompletedTests { get; set; }
    
    /// <summary>
    /// Promedio general de efectividad (%)
    /// </summary>
    public double GeneralAverage { get; set; }
    
    /// <summary>
    /// Número de tareas pendientes
    /// </summary>
    public int PendingTasks { get; set; }
    
    /// <summary>
    /// Información de la próxima sesión
    /// </summary>
    public NextSessionInfo? NextSession { get; set; }
}

/// <summary>
/// Información sobre la próxima sesión programada
/// </summary>
public class NextSessionInfo
{
    public DateTime SessionDate { get; set; }
    public string SessionTime { get; set; } = string.Empty;
    public string TeamName { get; set; } = string.Empty;
    public int TeamId { get; set; }
    public string SessionType { get; set; } = string.Empty; // "Entrenamiento", "Evaluación", etc.
}

/// <summary>
/// DTO para los atletas con mejor rendimiento (Top 5)
/// </summary>
public class TopPerformanceAthleteDto
{
    public int AthleteId { get; set; }
    public string AthleteName { get; set; } = string.Empty;
    public double PerformanceScore { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public DateTime LastEvaluationDate { get; set; }
}

/// <summary>
/// DTO para pruebas recientes
/// </summary>
public class RecentTestDto
{
    public int TestId { get; set; }
    public string AthleteName { get; set; } = string.Empty;
    public string TestType { get; set; } = string.Empty;
    public double Score { get; set; }
    public DateTime TestDate { get; set; }
    public string Status { get; set; } = string.Empty; // "Completada", "En progreso", etc.
}

/// <summary>
/// DTO para tareas pendientes
/// </summary>
public class PendingTaskDto
{
    public int TaskId { get; set; }
    public string TaskDescription { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty; // "Alta", "Media", "Baja"
    public DateTime DueDate { get; set; }
    public string TaskType { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
}

/// <summary>
/// DTO completo para el dashboard con todos los datos
/// </summary>
public class DashboardCompleteDto
{
    public DashboardIndicatorsDto Indicators { get; set; } = new DashboardIndicatorsDto();
    public List<TopPerformanceAthleteDto> TopPerformanceAthletes { get; set; } = new List<TopPerformanceAthleteDto>();
    public List<RecentTestDto> RecentTests { get; set; } = new List<RecentTestDto>();
    public List<PendingTaskDto> PendingTasks { get; set; } = new List<PendingTaskDto>();
    public List<MonthlyEvolutionDto> MonthlyEvolution { get; set; } = new List<MonthlyEvolutionDto>();
}

/// <summary>
/// DTO para la evolución mensual
/// </summary>
public class MonthlyEvolutionDto
{
    public string Month { get; set; } = string.Empty;
    public double AveragePerformance { get; set; }
    public int TestsCount { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}

/// <summary>
/// DTO para el resumen de equipo (overview)
/// </summary>
public class TeamOverviewDto
{
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public int TotalAthletes { get; set; }
    public int ActiveAthletes { get; set; }
    public int CompletedTests { get; set; }
    public double AveragePerformance { get; set; }
    public DateTime? LastTestDate { get; set; }
    public int PendingEvaluations { get; set; }
    public string Status { get; set; } = string.Empty; // "Activo", "Inactivo", "En evaluación"
}
