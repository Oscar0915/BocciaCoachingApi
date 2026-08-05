namespace BocciaCoaching.Models.DTO.Macrocycle
{
    public class MacrocycleSummaryDto
    {
        public Guid MacrocycleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AthleteName { get; set; } = string.Empty;
        public Guid AthleteId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EventCount { get; set; }
        public Guid CoachId { get; set; }
        public string? CoachName { get; set; }
        public Guid TeamId { get; set; }
        public string? TeamName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

