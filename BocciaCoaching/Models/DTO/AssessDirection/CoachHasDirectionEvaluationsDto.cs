namespace BocciaCoaching.Models.DTO.AssessDirection;

public class CoachHasDirectionEvaluationsDto
{
    /// <summary>
    /// Indica si el entrenador ha generado al menos una evaluación de dirección
    /// </summary>
    public bool HasEvaluations { get; set; }

    /// <summary>
    /// Número total de evaluaciones de dirección generadas por el entrenador
    /// </summary>
    public int TotalEvaluations { get; set; }

    /// <summary>
    /// ID del entrenador consultado
    /// </summary>
    public int CoachId { get; set; }
}

