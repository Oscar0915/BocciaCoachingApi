namespace BocciaCoaching.Models.DTO.AssessSaremas;

public class CoachHasSaremasEvaluationsDto
{
    /// <summary>
    /// Indica si el entrenador ha generado al menos una evaluación SAREMAS+
    /// </summary>
    public bool HasEvaluations { get; set; }

    /// <summary>
    /// Número total de evaluaciones SAREMAS+ generadas por el entrenador
    /// </summary>
    public int TotalEvaluations { get; set; }

    /// <summary>
    /// ID del entrenador consultado
    /// </summary>
    public int CoachId { get; set; }
}

