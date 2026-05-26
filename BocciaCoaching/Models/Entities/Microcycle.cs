using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Microcycle")]
    public class Microcycle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int MicrocycleId { get; set; }

        public string MacrocycleId { get; set; } = string.Empty;

        [ForeignKey("MacrocycleId")]
        public Macrocycle? Macrocycle { get; set; }

        /// <summary>Relación con el tipo de microciclo del catálogo (nullable)</summary>
        public string? MicrocycleTypeId { get; set; }

        [ForeignKey("MicrocycleTypeId")]
        public MicrocycleType? MicrocycleType { get; set; }

        public int Number { get; set; }

        public int WeekNumber { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        /// <summary>ordinario, choque, activacion, competitivo, recuperacion, descarga, evaluacion</summary>
        public string Type { get; set; } = string.Empty;

        public string? PeriodName { get; set; }

        public string? MesocycleName { get; set; }

        public bool HasPeakPerformance { get; set; }

        /// <summary>
        /// Porcentaje de carga semanal (0.0 a 1.0 = 0% a 100%).
        /// Corresponde al gráfico de barras del plan periodizado.
        /// </summary>
        public double LoadPercentage { get; set; }

        /// <summary>
        /// JSON: {"fisicaGeneral":0.15,"fisicaEspecial":0.15,"tecnica":0.20,"tactica":0.20,"teorica":0.20,"psicologica":0.10}
        /// </summary>
        [Column(TypeName = "text")]
        public string? TrainingDistribution { get; set; }

        /// <summary>Sesiones de entrenamiento del microciclo</summary>
        public ICollection<TrainingSession> TrainingSessions { get; set; } = new List<TrainingSession>();

        /// <summary>Días de la semana con sus porcentajes de lanzamiento para este microciclo</summary>
        public ICollection<MicrocycleDay> Days { get; set; } = new List<MicrocycleDay>();
    }
}

