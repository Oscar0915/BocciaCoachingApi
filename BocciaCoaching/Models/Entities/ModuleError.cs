using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("ModuleError")]
    public class ModuleError
    {
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ModuleErrorId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string Description { get; set; }
    }
}
