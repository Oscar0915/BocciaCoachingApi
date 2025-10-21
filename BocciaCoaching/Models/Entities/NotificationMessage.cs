using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("NotificationMessage")]
    public class NotificationMessage
    {
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NotificationMessageId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public bool? Status { get; set; } = true;
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string? Image {  get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int CoachId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public User Coach { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int AthleteId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public User Athlete { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int NotificationTypeId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public NotificationType NotificationType { get; set; }
    }
}
