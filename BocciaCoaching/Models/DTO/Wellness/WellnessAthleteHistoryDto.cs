namespace BocciaCoaching.Models.DTO.Wellness
{
    /// <summary>
    /// ES: Historial de tests diarios de bienestar de un atleta.
    /// </summary>
    public class WellnessAthleteHistoryDto
    {
        public Guid AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public int TotalRecords { get; set; }
        public double? OverallAverageScore { get; set; }
        public List<ResponseDailyWellnessDto> Records { get; set; } = new();
    }
}

