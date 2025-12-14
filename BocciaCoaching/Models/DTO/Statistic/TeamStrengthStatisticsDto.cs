namespace BocciaCoaching.Models.DTO.Statistic;

/// <summary>
/// DTO para las estadísticas de evaluación de fuerza filtradas por equipo
/// </summary>
public class TeamStrengthStatisticsDto
{
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public List<AthleteStatisticsDto> Athletes { get; set; } = new List<AthleteStatisticsDto>();
    public TeamAverageStatisticsDto TeamAverages { get; set; } = new TeamAverageStatisticsDto();
}

/// <summary>
/// Estadísticas individuales de cada atleta
/// </summary>
public class AthleteStatisticsDto
{
    public int AthleteId { get; set; }
    public string AthleteName { get; set; } = string.Empty;
    public int AssessStrengthId { get; set; }
    public DateTime EvaluationDate { get; set; }
    
    // Estadísticas generales
    public double EffectivenessPercentage { get; set; }
    public double AccuracyPercentage { get; set; }
    public int EffectiveThrow { get; set; }
    public int FailedThrow { get; set; }
    
    // Estadísticas por distancia - Cantidad de lanzamientos efectivos
    public int ShortThrow { get; set; }
    public int MediumThrow { get; set; }
    public double LongThrow { get; set; }
    
    // Porcentajes de efectividad por distancia
    public double ShortEffectivenessPercentage { get; set; }
    public double MediumEffectivenessPercentage { get; set; }
    public double LongEffectivenessPercentage { get; set; }
    
    // Precisión por distancia (puntos obtenidos)
    public int ShortThrowAccuracy { get; set; }
    public int MediumThrowAccuracy { get; set; }
    public int LongThrowAccuracy { get; set; }
    
    // Porcentajes de precisión por distancia
    public double ShortAccuracyPercentage { get; set; }
    public double MediumAccuracyPercentage { get; set; }
    public double LongAccuracyPercentage { get; set; }
}

/// <summary>
/// Estadísticas promedio del equipo
/// </summary>
public class TeamAverageStatisticsDto
{
    public double AverageEffectivenessPercentage { get; set; }
    public double AverageAccuracyPercentage { get; set; }
    public double AverageEffectiveThrow { get; set; }
    public double AverageFailedThrow { get; set; }
    
    // Promedios por distancia
    public double AverageShortThrow { get; set; }
    public double AverageMediumThrow { get; set; }
    public double AverageLongThrow { get; set; }
    
    // Promedios de efectividad por distancia
    public double AverageShortEffectivenessPercentage { get; set; }
    public double AverageMediumEffectivenessPercentage { get; set; }
    public double AverageLongEffectivenessPercentage { get; set; }
    
    // Promedios de precisión por distancia
    public double AverageShortAccuracyPercentage { get; set; }
    public double AverageMediumAccuracyPercentage { get; set; }
    public double AverageLongAccuracyPercentage { get; set; }
    
    public int TotalAthletes { get; set; }
}
