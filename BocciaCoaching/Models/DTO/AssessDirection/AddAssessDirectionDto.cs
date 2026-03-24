namespace BocciaCoaching.Models.DTO.AssessDirection
{
    public class AddAssessDirectionDto
    {
        public required string Description { get; set; }
        public required int TeamId { get; set; }
        public required int CoachId { get; set; }
    }
}

