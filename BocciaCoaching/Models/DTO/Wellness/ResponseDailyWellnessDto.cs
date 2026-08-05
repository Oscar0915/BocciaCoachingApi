namespace BocciaCoaching.Models.DTO.Wellness
{
    /// <summary>
    /// ES: Respuesta con los datos del test diario de bienestar registrado.
    /// </summary>
    public class ResponseDailyWellnessDto
    {
        public Guid DailyWellnessId { get; set; }
        public Guid AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public Guid? TeamId { get; set; }
        public DateTime AssessmentDate { get; set; }
        public int Sleep { get; set; }
        public int Stress { get; set; }
        public int Fatigue { get; set; }
        public int MusclePain { get; set; }
        public int TotalScore { get; set; }
        public double AverageScore { get; set; }
        public string? Observations { get; set; }
    }
}

