using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("AssessStrength")]
    public class AssessStrength
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AssessStrengthId { get; set; }

        public DateTime EvaluationDate { get; set; }

        public string Description { get; set; }

        public string State { get; set; }
    }
}
