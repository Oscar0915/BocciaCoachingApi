namespace BocciaCoaching.Models.DTO.AssessDirection
{
    /// <summary>
    /// Estadísticas individuales de cada atleta para control de dirección
    /// </summary>
    public class DirectionAthleteStatisticsDto
    {
        public int AthleteId { get; set; }
        public string AthleteName { get; set; } = string.Empty;
        public int AssessDirectionId { get; set; }
        public DateTime EvaluationDate { get; set; }

        // Estadísticas generales
        public double EffectivenessPercentage { get; set; }
        public double AccuracyPercentage { get; set; }
        public int EffectiveThrow { get; set; }
        public int FailedThrow { get; set; }

        // Estadísticas por distancia - Cantidad de lanzamientos efectivos
        public int ShortThrow { get; set; }
        public int MediumThrow { get; set; }
        public double LongThrow { get; set; }

        // Porcentajes de efectividad por distancia
        public double ShortEffectivenessPercentage { get; set; }
        public double MediumEffectivenessPercentage { get; set; }
        public double LongEffectivenessPercentage { get; set; }

        // Precisión por distancia (puntos obtenidos)
        public int ShortThrowAccuracy { get; set; }
        public int MediumThrowAccuracy { get; set; }
        public int LongThrowAccuracy { get; set; }

        // Porcentajes de precisión por distancia
        public double ShortAccuracyPercentage { get; set; }
        public double MediumAccuracyPercentage { get; set; }
        public double LongAccuracyPercentage { get; set; }

        // Estadísticas de desviación
        public int TotalDeviatedRight { get; set; }
        public int TotalDeviatedLeft { get; set; }
        public double DeviatedRightPercentage { get; set; }
        public double DeviatedLeftPercentage { get; set; }

        // Desviaciones por distancia
        public int ShortDeviatedRight { get; set; }
        public int ShortDeviatedLeft { get; set; }
        public int MediumDeviatedRight { get; set; }
        public int MediumDeviatedLeft { get; set; }
        public int LongDeviatedRight { get; set; }
        public int LongDeviatedLeft { get; set; }
    }
}

