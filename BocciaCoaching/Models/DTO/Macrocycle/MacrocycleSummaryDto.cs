namespace BocciaCoaching.Models.DTO.Macrocycle
{
    public class MacrocycleSummaryDto
    {
        public string MacrocycleId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string AthleteName { get; set; } = string.Empty;
        public int AthleteId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EventCount { get; set; }
        public int CoachId { get; set; }
        public string? CoachName { get; set; }
        public int TeamId { get; set; }
        public string? TeamName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

