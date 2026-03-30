using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("SessionSection")]
    public class SessionSection
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SessionSectionId { get; set; }

        public int SessionPartId { get; set; }

        [ForeignKey("SessionPartId")]
        public SessionPart? SessionPart { get; set; }

        /// <summary>Nombre de la sección</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Número de lanzamientos</summary>
        public int NumberOfThrows { get; set; }

        /// <summary>Estado de la sección: pendiente, en_proceso, completada, cancelada</summary>
        public string Status { get; set; } = "pendiente";

        /// <summary>true = diagonal propia, false = diagonal del rival</summary>
        public bool IsOwnDiagonal { get; set; } = true;

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        /// <summary>Observación del entrenador</summary>
        [Column(TypeName = "text")]
        public string? Observation { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}

