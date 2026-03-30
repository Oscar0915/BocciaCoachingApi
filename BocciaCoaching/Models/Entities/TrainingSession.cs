using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("TrainingSession")]
    public class TrainingSession
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TrainingSessionId { get; set; }

        /// <summary>Relación con el microciclo</summary>
        public int MicrocycleId { get; set; }

        [ForeignKey("MicrocycleId")]
        public Microcycle? Microcycle { get; set; }

        /// <summary>Día de la semana: lunes, martes, miercoles, jueves, viernes, sabado, domingo</summary>
        public string DayOfWeek { get; set; } = string.Empty;

        /// <summary>Duración en minutos</summary>
        public int Duration { get; set; }

        /// <summary>programada, en_proceso, terminada, finalizada, cancelada</summary>
        public string Status { get; set; } = "programada";

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        /// <summary>Ruta relativa de la evidencia fotográfica 1</summary>
        public string? PhotoEvidence1 { get; set; }

        /// <summary>Ruta relativa de la evidencia fotográfica 2</summary>
        public string? PhotoEvidence2 { get; set; }

        /// <summary>Ruta relativa de la evidencia fotográfica 3</summary>
        public string? PhotoEvidence3 { get; set; }

        /// <summary>Ruta relativa de la evidencia fotográfica 4</summary>
        public string? PhotoEvidence4 { get; set; }

        /// <summary>
        /// Porcentaje de lanzamientos de la sesión (ej: 25 = 25%).
        /// Si el 100% = TotalThrowsBase, entonces el máximo de lanzamientos = TotalThrowsBase * ThrowPercentage / 100
        /// </summary>
        public double ThrowPercentage { get; set; }

        /// <summary>Base total de lanzamientos que representa el 100% (ej: 1000)</summary>
        public int TotalThrowsBase { get; set; } = 1000;

        /// <summary>Máximo de lanzamientos calculado = TotalThrowsBase * ThrowPercentage / 100</summary>
        [NotMapped]
        public int MaxThrows => (int)(TotalThrowsBase * ThrowPercentage / 100);

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        /// <summary>Partes de la sesión (Propulsion, Saremas, 2x1, Escenarios de juego)</summary>
        public ICollection<SessionPart> Parts { get; set; } = new List<SessionPart>();
    }
}

