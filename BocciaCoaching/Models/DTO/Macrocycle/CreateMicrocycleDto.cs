namespace BocciaCoaching.Models.DTO.Macrocycle
{
    public class CreateMicrocycleDto
    {
        public int Number { get; set; }
        public int WeekNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        /// <summary>ordinario, choque, activacion, competitivo, recuperacion, descarga, evaluacion</summary>
        public string Type { get; set; } = string.Empty;

        public string? PeriodName { get; set; }
        public string? MesocycleName { get; set; }
        public bool HasPeakPerformance { get; set; }

        /// <summary>Carga semanal de 0.0 a 1.0 (0% a 100%). Si no se indica, se asigna automáticamente.</summary>
        public double LoadPercentage { get; set; }

        public TrainingDistributionDto? TrainingDistribution { get; set; }
    }
}
