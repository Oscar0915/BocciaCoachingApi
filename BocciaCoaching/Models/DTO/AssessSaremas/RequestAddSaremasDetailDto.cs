namespace BocciaCoaching.Models.DTO.AssessSaremas
{
    public class RequestAddSaremasDetailDto
    {
        public int ThrowNumber { get; set; }
        public string Diagonal { get; set; } = string.Empty;
        public string TechnicalComponent { get; set; } = string.Empty;
        public int ScoreObtained { get; set; }
        public string? Observations { get; set; }
        public string? FailureTags { get; set; }
        public bool Status { get; set; } = true;
        public int AthleteId { get; set; }
        public int SaremasEvalId { get; set; }

        // Datos de cancha (solo para "Salida")
        public double? WhiteBallX { get; set; }
        public double? WhiteBallY { get; set; }
        public double? ColorBallX { get; set; }
        public double? ColorBallY { get; set; }
        public double? EstimatedDistance { get; set; }
        public double? LaunchPointX { get; set; }
        public double? LaunchPointY { get; set; }
        public double? DistanceToLaunchPoint { get; set; }
    }
}

