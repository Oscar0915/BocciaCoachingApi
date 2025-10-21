using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("LogError")]
    public class LogError
    {
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int LogErrorId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int ModuleErrorId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public ModuleError ModuleError { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
