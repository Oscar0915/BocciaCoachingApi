using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Rol")]
    public class Rol
    {
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        [Key]
        public Guid RolId { get; set; }

        /// <summary>
        /// ES: Descripción del rol
        /// EN: 
        /// </summary>
        public required string Description { get; set; }
    }
}
