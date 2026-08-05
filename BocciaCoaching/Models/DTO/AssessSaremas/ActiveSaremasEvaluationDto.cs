namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class ActiveSaremasEvaluationDto
    {
        public Guid SaremasEvaluationId { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string? Description { get; set; }
        public string? State { get; set; }
        public Guid TeamId { get; set; }
        public string? TeamName { get; set; }
        public Guid CoachId { get; set; }
        public string? CoachName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<SaremasAthleteInEvaluationDto> Athletes { get; set; } = new();
        public List<SaremasThrowDto> Throws { get; set; } = new();
    }

    public class SaremasAthleteInEvaluationDto
    {
        public Guid AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public string? AthleteEmail { get; set; }
        public Guid CoachId { get; set; }
        public string? CoachName { get; set; }
    }

    public class SaremasThrowDto
    {
        public Guid SaremasThrowId { get; set; }
        public int ThrowNumber { get; set; }
        public string Diagonal { get; set; } = string.Empty;
        public string TechnicalComponent { get; set; } = string.Empty;
        public int ScoreObtained { get; set; }
        public string? Observations { get; set; }
        public string? FailureTags { get; set; }
        public bool Status { get; set; }
        public Guid AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public double? WhiteBallX { get; set; }
        public double? WhiteBallY { get; set; }
        public double? ColorBallX { get; set; }
        public double? ColorBallY { get; set; }
        public double? EstimatedDistance { get; set; }
        public double? LaunchPointX { get; set; }
        public double? LaunchPointY { get; set; }
        public double? DistanceToLaunchPoint { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

