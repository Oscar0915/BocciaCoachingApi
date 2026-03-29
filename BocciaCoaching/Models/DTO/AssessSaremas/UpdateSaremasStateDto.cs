namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class UpdateSaremasStateDto
    {
        public int Id { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string? Description { get; set; }
        public int TeamId { get; set; }
        public string? State { get; set; }

        public UpdateSaremasStateDto()
        {
            Id = 0;
            EvaluationDate = DateTime.Now;
            Description = string.Empty;
            TeamId = 0;
            State = string.Empty;
        }
    }
}

