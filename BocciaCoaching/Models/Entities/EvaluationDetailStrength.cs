using BocciaCoaching.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{

    [Table("EvaluationDetailStrength")]
    public class EvaluationDetailStrength: IAuditable
    {
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int EvaluationDetailStrengthId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int BoxNumber { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int ThrowOrder { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public decimal? TargetDistance { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public decimal? ScoreObtained { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string? Observations { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public bool Status { get; set; } = true;
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int AthleteId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public User Athlete {  get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int AssessStrengthId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public AssessStrength AssessStrength { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public DateTime CreatedAt { get ; set ; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public DateTime? UpdatedAt { get ; set ; }
    }
}
