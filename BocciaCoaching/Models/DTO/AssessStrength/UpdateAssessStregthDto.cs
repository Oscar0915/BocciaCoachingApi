namespace BocciaCoaching.Models.DTO.AssessStrength;

public class UpdateAssessStregthDto
{
    public int Id { get; init; } 
    public DateTime EvaluationDate { get; set; }
    public string? Description { get; set; }
    public int TeamId  { get; set; }
    public string? State { get; set; }

    // Constructor por defecto para inicializar propiedades y evitar advertencias del analizador
    public UpdateAssessStregthDto()
    {
        Id = 0;
        EvaluationDate = DateTime.Now;
        Description = string.Empty;
        TeamId = 0;
        State = string.Empty;
    }
}