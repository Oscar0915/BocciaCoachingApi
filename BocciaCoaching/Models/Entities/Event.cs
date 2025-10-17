using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    /// <summary>
    /// Tabla: Evento
    /// Table: Event
    /// </summary>
    [Table("Event")]
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int EventId { get; set; }
        public string? NameEvent {  get; set; }
        public string? DescriptionEvent {  get; set; }
        public string? Location {  get; set; }
        public string? Country { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public bool? Status { get; set; } = true;
        public int UserId { get; set; }
        public int LevelEventId { get; set; }
        public LevelEvent LevelEvent { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
