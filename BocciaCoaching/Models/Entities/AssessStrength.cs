using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("AssessStrength")]
    public class AssessStrength
    {
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AssessStrengthId { get; set; }

        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public DateTime EvaluationDate { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string State { get; set; }
    }
}
