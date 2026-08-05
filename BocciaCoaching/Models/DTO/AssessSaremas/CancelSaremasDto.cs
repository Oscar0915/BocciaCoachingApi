namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class CancelSaremasDto
    {
        public Guid SaremasEvalId { get; set; }
        public Guid CoachId { get; set; }
        public string? Reason { get; set; }
    }
}

