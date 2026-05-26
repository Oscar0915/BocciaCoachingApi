using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("MicrocycleType")]
    public class MicrocycleType
    {
        [Key]
        public string MicrocycleTypeId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>Nombre del tipo de microciclo: ordinario, choque, activación, competitivo, recuperación, descarga, evaluación</summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Código corto (símbolo) del tipo de microciclo, ej: μ1, μ2, μ3, μ4, μ5, μ6, μ7.
        /// Se usa para mostrar en el plan anual.
        /// </summary>
        [MaxLength(10)]
        public string? ShortCode { get; set; }

        /// <summary>Descripción del tipo de microciclo</summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>Estado activo/inactivo</summary>
        public bool Status { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        /// <summary>
        /// Configuraciones de días para este tipo de microciclo.
        /// Incluye tanto valores por defecto (CoachId == null) como overrides de coaches (CoachId != null).
        /// </summary>
        public ICollection<MicrocycleTypeDayDefault> DayConfigs { get; set; } = new List<MicrocycleTypeDayDefault>();
    }
}

