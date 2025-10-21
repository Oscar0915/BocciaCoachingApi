using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    /// <summary>
    /// Tabla: Nivel del evento
    /// Table: Level name
    /// </summary>
    [Table("LevelEvent")]
    public class LevelEvent
    {
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int LevelEventId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? NameLevel { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? Description {  get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public bool? Status { get; set; } = true;
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
