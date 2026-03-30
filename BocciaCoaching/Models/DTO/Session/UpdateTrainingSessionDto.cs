namespace BocciaCoaching.Models.DTO.Session
{
    public class UpdateTrainingSessionDto
    {
        public int TrainingSessionId { get; set; }

        /// <summary>programada, en_proceso, terminada, finalizada, cancelada</summary>
        public string? Status { get; set; }

        public int? Duration { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? ThrowPercentage { get; set; }
        public int? TotalThrowsBase { get; set; }
        public string? DayOfWeek { get; set; }
    }

    public class UpdateSessionSectionDto
    {
        public int SessionSectionId { get; set; }
        public string? Name { get; set; }
        public int? NumberOfThrows { get; set; }
        public string? Status { get; set; }
        public bool? IsOwnDiagonal { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Observation { get; set; }
    }

    public class AddSessionSectionDto
    {
        public int SessionPartId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int NumberOfThrows { get; set; }
        public bool IsOwnDiagonal { get; set; } = true;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Observation { get; set; }
    }
}

