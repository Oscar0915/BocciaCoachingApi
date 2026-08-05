namespace BocciaCoaching.Models.DTO.AssessDirection;

public class UpdateAssessDirectionDto
{
    public Guid Id { get; init; }
    public DateTime EvaluationDate { get; set; }
    public string? Description { get; set; }
    public Guid TeamId { get; set; }
    public string? State { get; set; }

    public UpdateAssessDirectionDto()
    {
        Id = Guid.Empty;
        EvaluationDate = DateTime.Now;
        Description = string.Empty;
        TeamId = Guid.Empty;
        State = string.Empty;
    }
}

