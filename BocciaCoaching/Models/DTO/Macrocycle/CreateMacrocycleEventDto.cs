namespace BocciaCoaching.Models.DTO.Macrocycle
{
    public class CreateMacrocycleEventDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
    }
}

