using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{

    [Table("AthletesToEvaluated")]
    public class AthletesToEvaluated
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AthletesToEvaluatedId { get; set; }

        public int CoachId { get; set; }
        public User Coach {  get; set; } 

        public int AthleteId { get; set; }
        public User Athlete { get; set; }

        public int AssessStrengthId { get; set; }

        public AssessStrength AssessStrength { get; set; }



    }
}
