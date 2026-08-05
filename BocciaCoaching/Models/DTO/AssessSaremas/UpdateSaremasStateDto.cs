namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class UpdateSaremasStateDto
    {
        public Guid Id { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string? Description { get; set; }
        public Guid TeamId { get; set; }
        public string? State { get; set; }

        public UpdateSaremasStateDto()
        {
            Id = Guid.Empty;
            EvaluationDate = DateTime.Now;
            Description = string.Empty;
            TeamId = Guid.Empty;
            State = string.Empty;
        }
    }
}

