using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities;
[Table("StrengthStatistics")]
public class StrengthStatistics
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int StrengthStatisticsId { get; set; }
    
    /// <summary>
    /// Porcentaje de precision
    /// </summary>
    public double EffectivenessPercentage { get; set; }
    
    /// <summary>
    /// Porcentaje de efectividad
    /// </summary>
    public double AccuracyPercentage { get; set; }
    /// <summary>
    /// Lanzamiento efectivo
    /// </summary>
    public int EffectiveThrow { get; set; }
    /// <summary>
    /// Lanzamiento fallido
    /// </summary>
    public int FailedThrow { get; set; }
    
    /// <summary>
    /// Lanzamiento efectivo corto
    /// </summary>
    public int ShortThrow { get; set; }
    
    /// <summary>
    /// Lanzamiento efectivo medio
    /// </summary>
    public int MediumThrow { get; set; }
    
    /// <summary>
    /// Lanzamiento efectivo largo
    /// </summary>
    public double LongThrow { get; set; }
    
    /// <summary>
    /// Porcentaje de efectividad corto
    /// </summary>
    public double ShortEffectivenessPercentage { get; set; }
    /// <summary>
    /// Porcentaje de efectividad medio
    /// </summary>
    public double MediumEffectivenessPercentage { get; set; }
    
    /// <summary>
    /// Porcentaje de efectividad largo
    /// </summary>
    public double LongEffectivenessPercentage { get; set; }
    
    /// <summary>
    /// Precisión de lanzamiento corto
    /// </summary>
    public int ShortThrowAccuracy  { get; set; }
    /// <summary>
    /// Precisión de lanzamiento medio
    /// </summary>
    public int MediumThrowAccuracy { get; set; }
    
    /// <summary>
    /// Precisión de lanzamiento largo
    /// </summary>
    public int LongThrowAccuracy { get; set; }
    
    /// <summary>
    /// Porcentaje de precisión corto
    /// </summary>
    public double ShortAccuracyPercentage { get; set; }
    /// <summary>
    /// Porcentaje de precisión medio
    /// </summary>
    public double MediumAccuracyPercentage { get; set; }
    /// <summary>
    /// Porcentaje de precisión largo
    /// </summary>
    public double LongAccuracyPercentage { get; set; }
    
    public int AssessStrengthId { get; set; }
    
    public AssessStrength? AssessStrength { get; set; }
    
    public int AthleteId { get; set; }
    
    public User? Athlete { get; set; }
}