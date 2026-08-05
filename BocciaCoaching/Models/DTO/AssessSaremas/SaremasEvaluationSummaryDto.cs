namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class SaremasEvaluationSummaryDto
    {
        public Guid SaremasEvaluationId { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string? Description { get; set; }
        public string? State { get; set; }
        public Guid TeamId { get; set; }
        public string? TeamName { get; set; }
        public Guid CoachId { get; set; }
        public string? CoachName { get; set; }
        public int AthletesCount { get; set; }
        public int ThrowsCount { get; set; }
        public int? TotalScore { get; set; }
        public double? AverageScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

