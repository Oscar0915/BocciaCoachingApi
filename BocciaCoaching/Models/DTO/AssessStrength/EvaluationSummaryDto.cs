namespace BocciaCoaching.Models.DTO.AssessStrength
{
    /// <summary>
    /// Resumen de una evaluación de fuerza
    /// </summary>
    public class EvaluationSummaryDto
    {
        public int AssessStrengthId { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string? Description { get; set; }
        public string? State { get; set; }
        public string? StateName { get; set; }
        public int TeamId { get; set; }
        public string? TeamName { get; set; }
        public int CoachId { get; set; }
        public string? CoachName { get; set; }
        public int AthletesCount { get; set; }
        public int ThrowsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
