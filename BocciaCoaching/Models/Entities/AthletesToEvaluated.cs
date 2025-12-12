using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{

    [Table("AthletesToEvaluated")]
    public class AthletesToEvaluated
    {
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AthletesToEvaluatedId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public int CoachId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public User? Coach {  get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public int AthleteId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public User? Athlete { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public int AssessStrengthId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public AssessStrength? AssessStrength { get; set; }



    }
}
