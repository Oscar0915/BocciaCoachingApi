using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    /// <summary>
    /// ES: Test diario de bienestar del atleta (cuestionario Wellness).
    /// Evalúa 4 indicadores en una escala de 1 (muy bajo/muy bueno) a 7 (muy alto/muy malo):
    /// Sueño, Estrés, Fatiga y Dolor Muscular. Se registra una vez por atleta por día.
    /// EN: Daily athlete wellness questionnaire.
    /// </summary>
    [Table("DailyWellness")]
    public class DailyWellness
    {
        [Key]
        public Guid DailyWellnessId { get; set; }

        /// <summary>
        /// ES: Identificador del atleta que responde el test
        /// </summary>
        public Guid AthleteId { get; set; }

        [ForeignKey("AthleteId")]
        public User? Athlete { get; set; }

        /// <summary>
        /// ES: Equipo al que pertenece el atleta (opcional)
        /// </summary>
        public Guid? TeamId { get; set; }

        [ForeignKey("TeamId")]
        public Team? Team { get; set; }

        /// <summary>
        /// ES: Fecha (día) a la que corresponde el test
        /// </summary>
        public DateTime AssessmentDate { get; set; }

        /// <summary>
        /// ES: Sueño (1 = muy muy bueno, 7 = muy muy malo)
        /// </summary>
        public int Sleep { get; set; }

        /// <summary>
        /// ES: Estrés (1 = muy muy bajo, 7 = muy muy alto)
        /// </summary>
        public int Stress { get; set; }

        /// <summary>
        /// ES: Fatiga (1 = muy muy bajo, 7 = muy muy alto)
        /// </summary>
        public int Fatigue { get; set; }

        /// <summary>
        /// ES: Dolor Muscular (1 = muy muy bajo, 7 = muy muy alto)
        /// </summary>
        public int MusclePain { get; set; }

        /// <summary>
        /// ES: Puntaje total (suma de los 4 indicadores, rango 4-28)
        /// </summary>
        public int TotalScore { get; set; }

        /// <summary>
        /// ES: Promedio de los 4 indicadores
        /// </summary>
        public double AverageScore { get; set; }

        /// <summary>
        /// ES: Observaciones o comentarios del atleta
        /// </summary>
        public string? Observations { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

