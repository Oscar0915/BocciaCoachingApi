using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    /// <summary>
    /// Configuración de porcentaje de lanzamiento por día para un tipo de microciclo.
    /// Si CoachId es null → valor por defecto global del sistema.
    /// Si CoachId tiene valor → override personalizado de ese coach específico.
    /// Esta tabla unifica lo que antes eran MicrocycleTypeDayDefault y CoachMicrocycleTypeDay.
    /// </summary>
    [Table("MicrocycleTypeDayDefault")]
    public class MicrocycleTypeDayDefault
    {
        [Key]
        public string MicrocycleTypeDayDefaultId { get; set; } = Guid.NewGuid().ToString();

        public string MicrocycleTypeId { get; set; } = string.Empty;

        [ForeignKey("MicrocycleTypeId")]
        public MicrocycleType? MicrocycleType { get; set; }

        /// <summary>
        /// NULL = valor por defecto global del sistema.
        /// Con valor = override personalizado del coach con ese Id.
        /// </summary>
        public int? CoachId { get; set; }

        [ForeignKey("CoachId")]
        public User? Coach { get; set; }

        /// <summary>Día de la semana: lunes, martes, miercoles, jueves, viernes, sabado, domingo</summary>
        [Required]
        [MaxLength(20)]
        public string DayOfWeek { get; set; } = string.Empty;

        /// <summary>Porcentaje de lanzamientos para ese día (ej: 25.0 = 25%)</summary>
        public double ThrowPercentage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
