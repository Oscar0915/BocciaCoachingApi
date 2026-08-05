namespace BocciaCoaching.Models.DTO.Macrocycle
{
    public class UpdateMacrocycleDto
    {
        public Guid MacrocycleId { get; set; }
        public Guid AthleteId { get; set; }
        public string AthleteName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Notes { get; set; }
        public Guid CoachId { get; set; }
        public Guid TeamId { get; set; }
        public List<CreateMacrocycleEventDto> Events { get; set; } = new();
    }
}

