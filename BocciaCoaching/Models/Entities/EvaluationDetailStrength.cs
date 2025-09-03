using BocciaCoaching.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{

    [Table("EvaluationDetailStrength")]
    public class EvaluationDetailStrength: IAuditable
    {
        public int EvaluationDetailStrengthId { get; set; }
        public int BoxNumber { get; set; }
        public int ThrowOrder { get; set; }
        public decimal? TargetDistance { get; set; }
        public decimal? ScoreObtained { get; set; }
        public string? Observations { get; set; }
        public bool Status { get; set; } = true;
        public int AthleteId { get; set; }
        public User Athlete {  get; set; }
        public int AssessStrengthId { get; set; }
        public AssessStrength AssessStrength { get; set; }
        public DateTime CreatedAt { get ; set ; }
        public DateTime? UpdatedAt { get ; set ; }
    }
}
