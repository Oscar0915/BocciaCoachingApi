using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("AssessStrength")]
    public class AssessStrength
    {
        public int AssessStrengthId { get; set; }

        public DateTime EvaluationDate { get; set; }
    }
}
