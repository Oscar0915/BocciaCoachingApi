namespace BocciaCoaching.Models.DTO.Macrocycle
{
    public class UpdateMacrocycleEventDto
    {
        public string MacrocycleEventId { get; set; } = string.Empty;
        public string MacrocycleId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
    }
}

