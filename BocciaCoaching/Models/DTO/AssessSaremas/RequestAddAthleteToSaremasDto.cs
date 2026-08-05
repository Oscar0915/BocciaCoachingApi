namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class RequestAddAthleteToSaremasDto
    {
        public Guid CoachId { get; set; }
        public Guid AthleteId { get; set; }
        public Guid SaremasEvalId { get; set; }
    }
}

