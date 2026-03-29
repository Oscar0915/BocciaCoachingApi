namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class SaremasStatisticsDto
    {
        public int EvaluationId { get; set; }
        public int TotalScore { get; set; }
        public int MaxPossibleScore { get; set; } = 140;
        public double AverageScore { get; set; }
        public int ThrowsCompleted { get; set; }

        public Dictionary<string, DiagonalStatsDto> ScoreByDiagonal { get; set; } = new();
        public Dictionary<string, ComponentStatsDto> ScoreByComponent { get; set; } = new();
        public Dictionary<string, BlockStatsDto> ScoreByBlock { get; set; } = new();
        public Dictionary<string, int> FailureTagFrequency { get; set; } = new();
        public SalidaMetricsDto? SalidaMetrics { get; set; }
    }

    public class DiagonalStatsDto
    {
        public int Total { get; set; }
        public double Average { get; set; }
        public int Count { get; set; }
    }

    public class ComponentStatsDto
    {
        public int Total { get; set; }
        public double Average { get; set; }
        public int Count { get; set; }
    }

    public class BlockStatsDto
    {
        public int Total { get; set; }
        public double Average { get; set; }
    }

    public class SalidaMetricsDto
    {
        public double AverageDistance { get; set; }
        public double AverageLaunchDistance { get; set; }
        public int ThrowsWithCourtData { get; set; }
    }
}

