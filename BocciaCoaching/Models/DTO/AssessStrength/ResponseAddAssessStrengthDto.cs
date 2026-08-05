namespace BocciaCoaching.Models.DTO.AssessStrength
{
    public class ResponseAddAssessStrengthDto
    {
        public ResponseAddAssessStrengthDto()
        {
            AssessStrengthId = Guid.Empty;
        }
        public Guid AssessStrengthId { get; set; }

        public DateTime DateEvaluation { get; set; }

        public bool State {  get; set; }
        
    }
}
