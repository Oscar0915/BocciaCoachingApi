namespace BocciaCoaching.Models.DTO.AssessDirection
{
    public class AddAssessDirectionDto
    {
        public required string Description { get; set; }
        public required Guid TeamId { get; set; }
        public required Guid CoachId { get; set; }
    }
}

