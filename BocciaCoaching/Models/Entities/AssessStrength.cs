using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("AssessStrength")]
    public class AssessStrength
    {
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AssessStrengthId { get; set; }

        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public DateTime EvaluationDate { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? State { get; set; }

        public int TeamId { get; set; }

        public Team? Team { get; set; }

        // Relation to the coach (User) who created the assessment
        /// <summary>
        /// ES: Id del entrenador que creó la prueba
        /// EN: Id of the coach who created the assessment
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// ES: Navegación al usuario (entrenador) que crea la prueba
        /// EN: Navigation to the user (coach) who created the assessment
        /// </summary>
        [ForeignKey("CoachId")]
        public User? Coach { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
