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
        /// JSON: {"fisicaGeneral":0.15,"fisicaEspecial":0.15,"tecnica":0.20,"tactica":0.20,"teorica":0.20,"psicologica":0.10}
        /// </summary>
        [Column(TypeName = "text")]
        public string? TrainingDistribution { get; set; }
    }
}

