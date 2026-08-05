namespace BocciaCoaching.Models.DTO.Statistic;

public class StrengthTestSummaryDto
{
    /// <summary>
    /// 
    /// </summary>
    public StrengthTestSummaryDto()
    {
        NameAthlete = string.Empty;
    }

    public string NameAthlete { get; set; }
    public Guid AthleteId { get; set; }
    public double EffectivenessPercentage { get; set; }
    public double AccuracyPercentage { get; set; }
    public Guid AssessStrengthId { get; set; }
    public Guid StrengthStatisticsId { get; set; }
}