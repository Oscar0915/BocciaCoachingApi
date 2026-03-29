using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("SaremasAthleteEvaluation")]
    public class SaremasAthleteEvaluation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SaremasAthleteEvaluationId { get; set; }

        public int SaremasEvalId { get; set; }

        [ForeignKey("SaremasEvalId")]
        public SaremasEvaluation? SaremasEvaluation { get; set; }

        public int AthleteId { get; set; }

        [ForeignKey("AthleteId")]
        public User? Athlete { get; set; }

        public string? AthleteName { get; set; }
    }
}

