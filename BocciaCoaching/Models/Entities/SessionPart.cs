using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("SessionPart")]
    public class SessionPart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SessionPartId { get; set; }

        public int TrainingSessionId { get; set; }

        [ForeignKey("TrainingSessionId")]
        public TrainingSession? TrainingSession { get; set; }

        /// <summary>Nombre de la parte: Propulsion, Saremas, 2x1, Escenarios de juego</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Orden de la parte (1-4)</summary>
        public int Order { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>Secciones de esta parte</summary>
        public ICollection<SessionSection> Sections { get; set; } = new List<SessionSection>();
    }
}

