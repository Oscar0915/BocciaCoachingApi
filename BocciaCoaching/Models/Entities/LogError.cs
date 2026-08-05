using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("LogError")]
    public class LogError
    {
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        [Key]
        public Guid LogErrorId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public Guid ModuleErrorId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public ModuleError ModuleError { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
