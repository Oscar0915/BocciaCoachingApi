namespace BocciaCoaching.Models.DTO.Macrocycle
{
    public class UpdateMicrocycleDto
    {
        public int MicrocycleId { get; set; }
        public string MacrocycleId { get; set; } = string.Empty;
        public string? Type { get; set; }
        public bool? HasPeakPerformance { get; set; }
        public TrainingDistributionDto? TrainingDistribution { get; set; }
    }
}

