using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("AssessDirection")]
    public class AssessDirection
    {
        /// <summary>
        /// ES: Identificador de la evaluación de control de dirección
        /// EN: Direction control assessment identifier
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AssessDirectionId { get; set; }

        /// <summary>
        /// ES: Fecha de la evaluación
        /// EN: Evaluation date
        /// </summary>
        public DateTime EvaluationDate { get; set; }

        /// <summary>
        /// ES: Descripción de la evaluación
        /// EN: Evaluation description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// ES: Estado de la evaluación (A=Activa, T=Terminada, C=Cancelada)
        /// EN: Evaluation state (A=Active, T=Finished, C=Cancelled)
        /// </summary>
        public string? State { get; set; }

        public int TeamId { get; set; }

        public Team? Team { get; set; }

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

