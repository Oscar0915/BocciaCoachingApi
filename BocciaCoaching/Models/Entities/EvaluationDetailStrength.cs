using BocciaCoaching.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{

    [Table("EvaluationDetailStrength")]
    public class EvaluationDetailStrength: IAuditable
    {
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int EvaluationDetailStrengthId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public int BoxNumber { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public int ThrowOrder { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public decimal? TargetDistance { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public decimal? ScoreObtained { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? Observations { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public bool Status { get; set; } = true;
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public int AthleteId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public User Athlete {  get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public int AssessStrengthId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public AssessStrength AssessStrength { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public DateTime CreatedAt { get ; set ; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public DateTime? UpdatedAt { get ; set ; }
    }
}
