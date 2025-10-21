using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{

    [Table("AthletesToEvaluated")]
    public class AthletesToEvaluated
    {
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AthletesToEvaluatedId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int CoachId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public User Coach {  get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int AthleteId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public User Athlete { get; set; }
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



    }
}
