using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("NotificationType")]
    public class NotificationType
    {
        /// <summary>
        /// ES: Identificador de la tabla
        /// EN: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NotificationTypeId { get; set; }
        /// <summary>
        /// ES: Nombre de la notificación
        /// EN: 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// ES: Descripción del tipo de la notificación
        /// EN: 
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// ES: Estado de la notificación
        /// EN: 
        /// </summary>
        public bool? Status { get; set; } = true;

        /// <summary>
        /// ES: Fecha de creación
        /// EN: 
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// ES: Fecha de actualización
        /// EN: 
        /// </summary>
        public DateTime? UpdatedAt { get; set; }


    }
}
