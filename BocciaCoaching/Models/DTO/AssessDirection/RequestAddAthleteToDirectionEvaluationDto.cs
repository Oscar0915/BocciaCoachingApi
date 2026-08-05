namespace BocciaCoaching.Models.DTO.AssessDirection
{
    public class RequestAddAthleteToDirectionEvaluationDto
    {
        public Guid CoachId { get; set; }
        public Guid AthleteId { get; set; }
        public Guid AssessDirectionId { get; set; }
    }
}

