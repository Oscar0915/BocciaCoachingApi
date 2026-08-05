namespace BocciaCoaching.Models.DTO.AssessStrength
{
    public class RequestAddAthleteToEvaluationDto
    {
        public Guid CoachId { get; set; }

        public Guid AthleteId { get; set; }

        public Guid AssessStrengthId { get; set; }
    }
}
