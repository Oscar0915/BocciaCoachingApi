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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int LevelEventId { get; set; }
        public string? NameLevel { get; set; }
        public string? Description {  get; set; }
        public bool? Status { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }
}
