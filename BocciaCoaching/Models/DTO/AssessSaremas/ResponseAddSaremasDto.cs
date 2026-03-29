namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class ResponseAddSaremasDto
    {
        public int SaremasEvaluationId { get; set; }
        public string? Description { get; set; }
        public int TeamId { get; set; }
        public int CoachId { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string? State { get; set; }
    }
}

