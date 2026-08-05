namespace BocciaCoaching.Models.DTO.AssessDirection
{
    public class CancelAssessDirectionDto
    {
        public Guid AssessDirectionId { get; set; }
        public Guid CoachId { get; set; }
        public string? Reason { get; set; }
    }
}

