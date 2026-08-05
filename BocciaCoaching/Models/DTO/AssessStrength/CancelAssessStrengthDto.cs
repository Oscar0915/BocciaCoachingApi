namespace BocciaCoaching.Models.DTO.AssessStrength
{
    public class CancelAssessStrengthDto
    {
        public Guid AssessStrengthId { get; set; }
        public Guid CoachId { get; set; }
        public string? Reason { get; set; }
    }
}

