using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("LogError")]
    public class LogError
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int LogErrorId { get; set; }
        public int ModuleErrorId { get; set; }
        public ModuleError ModuleError { get; set; }
        public string Location { get; set; }
        public string ErrorMessage { get; set; }
    }
}
