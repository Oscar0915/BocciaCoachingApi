namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class SaremasAthleteHistoryDto
    {
        public int AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public List<SaremasHistoryItemDto> Evaluations { get; set; } = new();
    }

    public class SaremasHistoryItemDto
    {
        public int SaremasEvaluationId { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string? Description { get; set; }
        public string? State { get; set; }
        public int? TotalScore { get; set; }
        public double? AverageScore { get; set; }
        public int ThrowsCompleted { get; set; }
        public string? TeamName { get; set; }
        public string? CoachName { get; set; }
    }
}

