namespace BocciaCoaching.Models.DTO.AssessStrength;

public class UpdateAssessStregthDto
{
    public int Id { get; set; } 
    public DateTime EvaluationDate { get; set; }
    public string Description { get; set; }
    public int TeamId  { get; set; }
    public string State { get; set; }
}