namespace BocciaCoaching.Models.DTO.Wellness
{
    /// <summary>
    /// ES: Historial de cuestionarios de bienestar de un atleta.
    /// Los promedios se calculan solo con registros McLean (escala 1-5): mezclar escalas no tendría sentido.
    /// </summary>
    public class WellnessAthleteHistoryDto
    {
        public Guid AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public int TotalRecords { get; set; }

        /// <summary>Promedio del puntaje total (5-25) de los registros McLean.</summary>
        public double? AverageTotalScore { get; set; }

        /// <summary>Promedio por ítem (1-5) de los registros McLean.</summary>
        public double? OverallAverageScore { get; set; }

        public int GreenDays { get; set; }
        public int YellowDays { get; set; }
        public int RedDays { get; set; }

        public List<ResponseDailyWellnessDto> Records { get; set; } = new();
    }
}
