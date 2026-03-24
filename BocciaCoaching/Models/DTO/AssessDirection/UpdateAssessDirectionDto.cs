namespace BocciaCoaching.Models.DTO.AssessDirection;

public class UpdateAssessDirectionDto
{
    public int Id { get; init; }
    public DateTime EvaluationDate { get; set; }
    public string? Description { get; set; }
    public int TeamId { get; set; }
    public string? State { get; set; }

    public UpdateAssessDirectionDto()
    {
        Id = 0;
        EvaluationDate = DateTime.Now;
        Description = string.Empty;
        TeamId = 0;
        State = string.Empty;
    }
}

