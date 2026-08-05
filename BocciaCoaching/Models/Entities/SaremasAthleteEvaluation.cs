using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("SaremasAthleteEvaluation")]
    public class SaremasAthleteEvaluation
    {
        [Key]
        public Guid SaremasAthleteEvaluationId { get; set; }

        public Guid SaremasEvalId { get; set; }

        [ForeignKey("SaremasEvalId")]
        public SaremasEvaluation? SaremasEvaluation { get; set; }

        public Guid AthleteId { get; set; }

        [ForeignKey("AthleteId")]
        public User? Athlete { get; set; }

        public string? AthleteName { get; set; }
    }
}

