namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class ResponseAddSaremasDto
    {
        public Guid SaremasEvaluationId { get; set; }
        public string? Description { get; set; }
        public Guid TeamId { get; set; }
        public Guid CoachId { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string? State { get; set; }
    }
}

