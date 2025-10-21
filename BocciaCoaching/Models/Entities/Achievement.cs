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
        /// Campo:
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AchievementId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public bool? Status { get; set; } = true;
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int Ranked {  get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public Event? Event { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
