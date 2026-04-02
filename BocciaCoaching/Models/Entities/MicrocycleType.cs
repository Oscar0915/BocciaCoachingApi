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

        /// <summary>Descripción del tipo de microciclo</summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>Estado activo/inactivo</summary>
        public bool Status { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<MicrocycleTypeDayDefault> DefaultDays { get; set; } = new List<MicrocycleTypeDayDefault>();
        public ICollection<CoachMicrocycleTypeDay> CoachDays { get; set; } = new List<CoachMicrocycleTypeDay>();
    }
}

