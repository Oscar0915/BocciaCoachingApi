namespace BocciaCoaching.Models.DTO.AssessStrength
{
    public class ResponseAddAssessStrengthDto
    {
        public ResponseAddAssessStrengthDto()
        {
            AssessStrengthId = 0;
        }
        public int AssessStrengthId { get; set; }

        public DateTime DateEvaluation { get; set; }

        public bool State {  get; set; }
        
    }
}
