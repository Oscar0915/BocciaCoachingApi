using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Messages")]
    public class Message
    {
        [Key]
        [MaxLength(255)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(255)]
        public string ConversationId { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string SenderId { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string SenderName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string SenderPhoto { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string Text { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; }

        // Relación con Conversation - Cascade Delete se configurará en la migración
        [ForeignKey("ConversationId")]
        public virtual Conversation? Conversation { get; set; }
    }
}

