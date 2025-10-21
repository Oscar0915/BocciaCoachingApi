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
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int EventId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string? NameEvent {  get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string? DescriptionEvent {  get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string? Location {  get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public bool? Status { get; set; } = true;
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int LevelEventId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public LevelEvent LevelEvent { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
