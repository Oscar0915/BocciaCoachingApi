namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class AddSaremasEvaluationDto
    {
        public required string Description { get; set; }
        public required Guid TeamId { get; set; }
        public required Guid CoachId { get; set; }
    }
}

