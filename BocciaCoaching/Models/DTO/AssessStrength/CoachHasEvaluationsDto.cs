namespace BocciaCoaching.Models.DTO.AssessStrength;

public class CoachHasEvaluationsDto
{
    /// <summary>
    /// Indica si el entrenador ha generado al menos una evaluación de fuerza
    /// </summary>
    public bool HasEvaluations { get; set; }

    /// <summary>
    /// Número total de evaluaciones generadas por el entrenador
    /// </summary>
    public int TotalEvaluations { get; set; }

    /// <summary>
    /// ID del entrenador consultado
    /// </summary>
    public int CoachId { get; set; }
}

