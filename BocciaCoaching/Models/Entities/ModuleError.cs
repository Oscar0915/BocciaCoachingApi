using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("ModuleError")]
    public class ModuleError
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ModuleErrorId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
