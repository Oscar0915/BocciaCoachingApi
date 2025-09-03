using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Rol")]
    public class Rol
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int RolId { get; set; }

        public required string Description { get; set; }
    }
}
