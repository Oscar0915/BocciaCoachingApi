using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    /// <summary>
    /// Distribución de componentes de entrenamiento personalizada por entrenador
    /// para cada tipo de microciclo. Permite que cada coach defina sus propios
    /// porcentajes de Física General, Física Especial, Técnica, Táctica, Teórica
    /// y Psicológica como valores por defecto al generar nuevos macrociclos.
    /// Solo afecta al coach específico que la configuró.
    /// </summary>
    [Table("CoachMicrocycleTypeDistribution")]
    public class CoachMicrocycleTypeDistribution
    {
        [Key]
        public string CoachMicrocycleTypeDistributionId { get; set; } = Guid.NewGuid().ToString();

        public int CoachId { get; set; }

        [ForeignKey("CoachId")]
        public User? Coach { get; set; }

        public string MicrocycleTypeId { get; set; } = string.Empty;

        [ForeignKey("MicrocycleTypeId")]
        public MicrocycleType? MicrocycleType { get; set; }

        /// <summary>Porcentaje de Física General (0.0 a 1.0)</summary>
        public double FisicaGeneral { get; set; }

        /// <summary>Porcentaje de Física Especial (0.0 a 1.0)</summary>
        public double FisicaEspecial { get; set; }

        /// <summary>Porcentaje de Técnica (0.0 a 1.0)</summary>
        public double Tecnica { get; set; }

        /// <summary>Porcentaje de Táctica (0.0 a 1.0)</summary>
        public double Tactica { get; set; }

        /// <summary>Porcentaje de Teórica (0.0 a 1.0)</summary>
        public double Teorica { get; set; }

        /// <summary>Porcentaje de Psicológica (0.0 a 1.0)</summary>
        public double Psicologica { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}

