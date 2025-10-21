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
        /// ES:
        /// EN: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int EventId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? NameEvent {  get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? DescriptionEvent {  get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? Location {  get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public bool? Status { get; set; } = true;
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public int LevelEventId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public LevelEvent LevelEvent { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
