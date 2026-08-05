using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("ModuleError")]
    public class ModuleError
    {
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        [Key]
        public Guid ModuleErrorId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string Description { get; set; }
    }
}
