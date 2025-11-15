namespace BocciaCoaching.Models.DTO.AssessStrength
{
    public class AddAssessStrengthDto
    {
        public int Id { get; set; } 
        public DateTime EvaluationDate { get; set; }
        public string Description { get; set; }
        public int TeamId  { get; set; }
        public string State { get; set; }
    }
}
