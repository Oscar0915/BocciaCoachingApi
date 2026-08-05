namespace BocciaCoaching.Models.DTO.AssessDirection
{
    /// <summary>
    /// Resumen de una evaluación de control de dirección
    /// </summary>
    public class DirectionEvaluationSummaryDto
    {
        public Guid AssessDirectionId { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string? Description { get; set; }
        public string? State { get; set; }
        public string? StateName { get; set; }
        public Guid TeamId { get; set; }
        public string? TeamName { get; set; }
        public Guid CoachId { get; set; }
        public string? CoachName { get; set; }
        public int AthletesCount { get; set; }
        public int ThrowsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

