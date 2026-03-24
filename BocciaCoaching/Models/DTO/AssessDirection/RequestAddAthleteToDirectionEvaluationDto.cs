namespace BocciaCoaching.Models.DTO.AssessDirection
{
    public class RequestAddAthleteToDirectionEvaluationDto
    {
        public int CoachId { get; set; }
        public int AthleteId { get; set; }
        public int AssessDirectionId { get; set; }
    }
}

