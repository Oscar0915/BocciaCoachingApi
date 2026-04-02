using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("CoachMicrocycleTypeDay")]
    public class CoachMicrocycleTypeDay
    {
        [Key]
        public string CoachMicrocycleTypeDayId { get; set; } = Guid.NewGuid().ToString();

        public int CoachId { get; set; }

        [ForeignKey("CoachId")]
        public User? Coach { get; set; }

        public string MicrocycleTypeId { get; set; } = string.Empty;

        [ForeignKey("MicrocycleTypeId")]
        public MicrocycleType? MicrocycleType { get; set; }

        /// <summary>Día de la semana: lunes, martes, miercoles, jueves, viernes, sabado, domingo</summary>
        [Required]
        [MaxLength(20)]
        public string DayOfWeek { get; set; } = string.Empty;

        /// <summary>Porcentaje personalizado de lanzamientos para ese día</summary>
        public double ThrowPercentage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}

