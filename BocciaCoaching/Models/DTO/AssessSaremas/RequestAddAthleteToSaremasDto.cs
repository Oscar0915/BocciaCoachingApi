namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class RequestAddAthleteToSaremasDto
    {
        public int CoachId { get; set; }
        public int AthleteId { get; set; }
        public int SaremasEvalId { get; set; }
    }
}

