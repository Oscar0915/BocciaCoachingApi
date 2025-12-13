using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Conversations")]
    public class Conversation
    {
        [Key]
        [MaxLength(255)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Column(TypeName = "longtext")]
        public string Participants { get; set; } = string.Empty; // JSON string de array de IDs

        [Required]
        [Column(TypeName = "longtext")]
        public string ParticipantsData { get; set; } = string.Empty; // JSON string de ParticipantData[]

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(255)]
        public string? LastMessageId { get; set; }

        public int UnreadCount { get; set; } = 0;

        // Navegación - Relación 1:N con Messages (Cascade Delete se configura en Message.cs)
        [InverseProperty("Conversation")]
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

        // Navegación - Relación 1:1 con LastMessage (Restrict Delete requiere OnModelCreating)
        [ForeignKey("LastMessageId")]
        public virtual Message? LastMessage { get; set; }
    }

    public class ParticipantData
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "coach" or "athlete"
        public bool IsOnline { get; set; } = false;
    }
}

