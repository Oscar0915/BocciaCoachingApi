using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("MicrocycleTypeDayDefault")]
    public class MicrocycleTypeDayDefault
    {
        [Key]
        public string MicrocycleTypeDayDefaultId { get; set; } = Guid.NewGuid().ToString();

        public string MicrocycleTypeId { get; set; } = string.Empty;

        [ForeignKey("MicrocycleTypeId")]
        public MicrocycleType? MicrocycleType { get; set; }

        /// <summary>Día de la semana: lunes, martes, miercoles, jueves, viernes, sabado, domingo</summary>
        [Required]
        [MaxLength(20)]
        public string DayOfWeek { get; set; } = string.Empty;

        /// <summary>Porcentaje de lanzamientos para ese día (ej: 25.0 = 25%)</summary>
        public double ThrowPercentage { get; set; }
    }
}

