using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    /// <summary>
    /// Representa las configuraciones de días de la semana con sus porcentajes de lanzamiento
    /// para una instancia concreta de microciclo. Se poblan desde el catálogo del tipo de microciclo
    /// y pueden ser personalizadas por el coach.
    /// </summary>
    [Table("MicrocycleDay")]
    public class MicrocycleDay
    {
        [Key]
        public string MicrocycleDayId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>Relación con el microciclo concreto</summary>
        public int MicrocycleId { get; set; }

        [ForeignKey("MicrocycleId")]
        public Microcycle? Microcycle { get; set; }

        /// <summary>Día de la semana: lunes, martes, miercoles, jueves, viernes, sabado, domingo</summary>
        [Required]
        [MaxLength(20)]
        public string DayOfWeek { get; set; } = string.Empty;

        /// <summary>Porcentaje de lanzamientos para ese día (ej: 25.0 = 25%)</summary>
        public double ThrowPercentage { get; set; }

        /// <summary>false = valor heredado del catálogo del tipo de microciclo, true = personalizado por el coach</summary>
        public bool IsCustom { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}

