namespace BocciaCoaching.Models.DTO.AssessDirection
{
    public class ResponseAddAssessDirectionDto
    {
        public ResponseAddAssessDirectionDto()
        {
            AssessDirectionId = Guid.Empty;
        }
        public Guid AssessDirectionId { get; set; }
        public DateTime DateEvaluation { get; set; }
        public bool State { get; set; }
    }
}

