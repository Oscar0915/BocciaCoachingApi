namespace BocciaCoaching.Models.DTO.AssessDirection
{
    public class CancelAssessDirectionDto
    {
        public int AssessDirectionId { get; set; }
        public int CoachId { get; set; }
        public string? Reason { get; set; }
    }
}

