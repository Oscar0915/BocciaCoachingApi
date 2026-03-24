using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities;

[Table("DirectionStatistics")]
public class DirectionStatistics
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int DirectionStatisticsId { get; set; }

    /// <summary>
    /// Porcentaje de precisión general
    /// </summary>
    public double EffectivenessPercentage { get; set; }

    /// <summary>
    /// Porcentaje de efectividad general
    /// </summary>
    public double AccuracyPercentage { get; set; }

    /// <summary>
    /// Lanzamientos efectivos
    /// </summary>
    public int EffectiveThrow { get; set; }

    /// <summary>
    /// Lanzamientos fallidos
    /// </summary>
    public int FailedThrow { get; set; }

    /// <summary>
    /// Lanzamientos efectivos a distancia corta (3 metros)
    /// </summary>
    public int ShortThrow { get; set; }

    /// <summary>
    /// Lanzamientos efectivos a distancia media (6 metros)
    /// </summary>
    public int MediumThrow { get; set; }

    /// <summary>
    /// Lanzamientos efectivos a distancia larga (9 metros)
    /// </summary>
    public double LongThrow { get; set; }

    /// <summary>
    /// Porcentaje de efectividad corto (3 metros)
    /// </summary>
    public double ShortEffectivenessPercentage { get; set; }

    /// <summary>
    /// Porcentaje de efectividad medio (6 metros)
    /// </summary>
    public double MediumEffectivenessPercentage { get; set; }

    /// <summary>
    /// Porcentaje de efectividad largo (9 metros)
    /// </summary>
    public double LongEffectivenessPercentage { get; set; }

    /// <summary>
    /// Precisión de lanzamiento corto
    /// </summary>
    public int ShortThrowAccuracy { get; set; }

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

    /// <summary>
    /// Total de lanzamientos desviados a la derecha
    /// </summary>
    public int TotalDeviatedRight { get; set; }

    /// <summary>
    /// Total de lanzamientos desviados a la izquierda
    /// </summary>
    public int TotalDeviatedLeft { get; set; }

    /// <summary>
    /// Porcentaje de desviación a la derecha
    /// </summary>
    public double DeviatedRightPercentage { get; set; }

    /// <summary>
    /// Porcentaje de desviación a la izquierda
    /// </summary>
    public double DeviatedLeftPercentage { get; set; }

    /// <summary>
    /// Desviaciones a la derecha en distancia corta (3m)
    /// </summary>
    public int ShortDeviatedRight { get; set; }

    /// <summary>
    /// Desviaciones a la izquierda en distancia corta (3m)
    /// </summary>
    public int ShortDeviatedLeft { get; set; }

    /// <summary>
    /// Desviaciones a la derecha en distancia media (6m)
    /// </summary>
    public int MediumDeviatedRight { get; set; }

    /// <summary>
    /// Desviaciones a la izquierda en distancia media (6m)
    /// </summary>
    public int MediumDeviatedLeft { get; set; }

    /// <summary>
    /// Desviaciones a la derecha en distancia larga (9m)
    /// </summary>
    public int LongDeviatedRight { get; set; }

    /// <summary>
    /// Desviaciones a la izquierda en distancia larga (9m)
    /// </summary>
    public int LongDeviatedLeft { get; set; }

    public int AssessDirectionId { get; set; }

    public AssessDirection? AssessDirection { get; set; }

    public int AthleteId { get; set; }

    public User? Athlete { get; set; }
}

