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
    public int AthleteId { get; set; }
    public double EffectivenessPercentage { get; set; }
    public double AccuracyPercentage { get; set; }
    public int AssessStrengthId { get; set; }
    public int StrengthStatisticsId { get; set; }
}