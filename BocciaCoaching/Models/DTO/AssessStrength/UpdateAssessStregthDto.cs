namespace BocciaCoaching.Models.DTO.AssessStrength;

public class UpdateAssessStregthDto
{
    public Guid Id { get; init; } 
    public DateTime EvaluationDate { get; set; }
    public string? Description { get; set; }
    public Guid TeamId  { get; set; }
    public string? State { get; set; }

    // Constructor por defecto para inicializar propiedades y evitar advertencias del analizador
    public UpdateAssessStregthDto()
    {
        Id = Guid.Empty;
        EvaluationDate = DateTime.Now;
        Description = string.Empty;
        TeamId = Guid.Empty;
        State = string.Empty;
    }
}