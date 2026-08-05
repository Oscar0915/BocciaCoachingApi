using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    /// <summary>
    /// Tabla: Logros
    /// Table: Achievement
    /// </summary>
    [Table("Achievement")]
    public class Achievement
    {
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        [Key]
        public Guid AchievementId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public bool? Status { get; set; } = true;
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public int Ranked {  get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public Guid EventId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public Event? Event { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
