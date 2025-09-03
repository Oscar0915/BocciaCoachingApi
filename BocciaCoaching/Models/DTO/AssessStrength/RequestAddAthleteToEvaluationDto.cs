namespace BocciaCoaching.Models.DTO.AssessStrength
{
    public class RequestAddAthleteToEvaluationDto
    {
        public int CoachId { get; set; }

        public int AthleteId { get; set; }

        public int AssessStrengthId { get; set; }
    }
}
