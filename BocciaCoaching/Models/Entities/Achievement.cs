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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AchievementId { get; set; }
        public bool? Status { get; set; } = true;
        public int Ranked {  get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
