using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("AssessStrength")]
    public class AssessStrength
    {
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AssessStrengthId { get; set; }

        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public DateTime EvaluationDate { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string State { get; set; }
    }
}
