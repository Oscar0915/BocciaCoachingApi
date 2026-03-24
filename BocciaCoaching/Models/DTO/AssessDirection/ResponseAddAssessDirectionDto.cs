namespace BocciaCoaching.Models.DTO.AssessDirection
{
    public class ResponseAddAssessDirectionDto
    {
        public ResponseAddAssessDirectionDto()
        {
            AssessDirectionId = 0;
        }
        public int AssessDirectionId { get; set; }
        public DateTime DateEvaluation { get; set; }
        public bool State { get; set; }
    }
}

