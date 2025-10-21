using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Rol")]
    public class Rol
    {
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int RolId { get; set; }

        /// <summary>
        /// Campo: Descripción del rol
        /// Field: 
        /// </summary>
        public required string Description { get; set; }
    }
}
