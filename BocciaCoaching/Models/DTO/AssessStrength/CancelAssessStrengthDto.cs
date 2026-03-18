namespace BocciaCoaching.Models.DTO.AssessStrength
{
    public class CancelAssessStrengthDto
    {
        public int AssessStrengthId { get; set; }
        public int CoachId { get; set; }
        public string? Reason { get; set; }
    }
}

