using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("AthletesToEvaluatedDirection")]
    public class AthletesToEvaluatedDirection
    {
        /// <summary>
        /// ES: Identificador
        /// EN: Identifier
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AthletesToEvaluatedDirectionId { get; set; }

        /// <summary>
        /// ES: Identificador del entrenador
        /// EN: Coach identifier
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// ES: Navegación al entrenador
        /// EN: Navigation to coach
        /// </summary>
        public User? Coach { get; set; }

        /// <summary>
        /// ES: Identificador del atleta
        /// EN: Athlete identifier
        /// </summary>
        public int AthleteId { get; set; }

        /// <summary>
        /// ES: Navegación al atleta
        /// EN: Navigation to athlete
        /// </summary>
        public User? Athlete { get; set; }

        /// <summary>
        /// ES: Identificador de la evaluación de dirección
        /// EN: Direction assessment identifier
        /// </summary>
        public int AssessDirectionId { get; set; }

        /// <summary>
        /// ES: Navegación a la evaluación de dirección
        /// EN: Navigation to direction assessment
        /// </summary>
        public AssessDirection? AssessDirection { get; set; }
    }
}

