using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("NotificationType")]
    public class NotificationType
    {
        /// <summary>
        /// Campo: Identificador de la tabla
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NotificationTypeId { get; set; }
        /// <summary>
        /// Campo: Nombre de la notificación
        /// Field: 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Campo: Descripción del tipo de la notificación
        /// Field: 
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Campo: Estado de la notificación
        /// Field: 
        /// </summary>
        public bool? Status { get; set; } = true;

        /// <summary>
        /// Campo: Fecha de creación
        /// Field: 
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Campo: Fecha de actualización
        /// Field: 
        /// </summary>
        public DateTime? UpdatedAt { get; set; }


    }
}
