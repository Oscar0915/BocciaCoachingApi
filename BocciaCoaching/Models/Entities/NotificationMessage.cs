using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("NotificationMessage")]
    public class NotificationMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NotificationMessageId { get; set; }
        public string? Message { get; set; }
        public bool? Status { get; set; } = true;
        public string? Image {  get; set; }
        public int CoachId { get; set; }
        public User Coach { get; set; }
        public int AthleteId { get; set; }
        public User Athlete { get; set; }

        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
