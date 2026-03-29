namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class AddSaremasEvaluationDto
    {
        public required string Description { get; set; }
        public required int TeamId { get; set; }
        public required int CoachId { get; set; }
    }
}

