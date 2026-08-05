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
        [Key]
        public Guid AthletesToEvaluatedId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public Guid CoachId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public User? Coach {  get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public Guid AthleteId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public User? Athlete { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public Guid AssessStrengthId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public AssessStrength? AssessStrength { get; set; }



    }
}
