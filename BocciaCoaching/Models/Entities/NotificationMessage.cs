using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("NotificationMessage")]
    public class NotificationMessage
    {
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NotificationMessageId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public bool? Status { get; set; } = true;
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? Image {  get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public int CoachId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public User Coach { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public int AthleteId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public User Athlete { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public int NotificationTypeId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public NotificationType NotificationType { get; set; }
    }
}
