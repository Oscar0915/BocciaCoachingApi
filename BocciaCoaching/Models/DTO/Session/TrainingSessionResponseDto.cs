namespace BocciaCoaching.Models.DTO.Session
{
    public class TrainingSessionResponseDto
    {
        public int TrainingSessionId { get; set; }
        public int MicrocycleId { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? PhotoEvidence1 { get; set; }
        public string? PhotoEvidence2 { get; set; }
        public string? PhotoEvidence3 { get; set; }
        public string? PhotoEvidence4 { get; set; }
        public double ThrowPercentage { get; set; }
        public int TotalThrowsBase { get; set; }
        public int MaxThrows { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<SessionPartResponseDto> Parts { get; set; } = new();
    }

    public class SessionPartResponseDto
    {
        public int SessionPartId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SessionSectionResponseDto> Sections { get; set; } = new();
    }

    public class SessionSectionResponseDto
    {
        public int SessionSectionId { get; set; }
        public int SessionPartId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int NumberOfThrows { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsOwnDiagonal { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Observation { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class TrainingSessionSummaryDto
    {
        public int TrainingSessionId { get; set; }
        public int MicrocycleId { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Status { get; set; } = string.Empty;
        public double ThrowPercentage { get; set; }
        public int MaxThrows { get; set; }
        public int TotalParts { get; set; }
        public int TotalSections { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

