using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("SaremasEvaluation")]
    public class SaremasEvaluation
    {
        [Key]
        public Guid SaremasEvaluationId { get; set; }

        public string? Description { get; set; }

        public Guid TeamId { get; set; }
        public Team? Team { get; set; }

        public Guid CoachId { get; set; }

        [ForeignKey("CoachId")]
        public User? Coach { get; set; }

        public DateTime EvaluationDate { get; set; }

        /// <summary>
        /// Estado: Active, Completed, Cancelled
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Puntaje total (calculado al completar, máx 140)
        /// </summary>
        public int? TotalScore { get; set; }

        /// <summary>
        /// Promedio por tiro (calculado)
        /// </summary>
        public double? AverageScore { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<SaremasAthleteEvaluation> Athletes { get; set; } = new List<SaremasAthleteEvaluation>();
        public ICollection<SaremasThrow> Throws { get; set; } = new List<SaremasThrow>();
    }
}

