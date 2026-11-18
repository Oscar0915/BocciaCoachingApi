using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    /// <summary>
    /// ES: Cuestionario diario de bienestar del atleta.
    /// Versión actual (<see cref="ScaleVersion"/> = 2): escala de McLean et al. (2010), derivada del
    /// Índice de Hooper. 5 ítems (Fatiga, Sueño, Dolor muscular, Estrés, Estado de ánimo) puntuados
    /// de 1 (pésimo) a 5 (excelente); total 5-25. Un registro por atleta por día.
    /// Los registros con <see cref="ScaleVersion"/> = 1 son del cuestionario anterior (4 ítems, 1-7,
    /// donde 1 era el mejor estado) y se conservan sin convertir.
    /// </summary>
    [Table("DailyWellness")]
    public class DailyWellness
    {
        /// <summary>Cuestionario anterior: 4 ítems, escala 1-7 (1 = mejor).</summary>
        public const int LegacyScaleVersion = 1;

        /// <summary>McLean (2010): 5 ítems, escala 1-5 (5 = mejor).</summary>
        public const int McLeanScaleVersion = 2;

        [Key]
        public Guid DailyWellnessId { get; set; }

        /// <summary>ES: Atleta que responde el cuestionario.</summary>
        public Guid AthleteId { get; set; }

        [ForeignKey("AthleteId")]
        public User? Athlete { get; set; }

        /// <summary>ES: Equipo en cuyo contexto se respondió (opcional).</summary>
        public Guid? TeamId { get; set; }

        [ForeignKey("TeamId")]
        public Team? Team { get; set; }

        /// <summary>ES: Día local del atleta al que corresponde el cuestionario (hora 00:00).</summary>
        public DateTime AssessmentDate { get; set; }

        /// <summary>ES: Versión de la escala con la que se registró (1 = anterior, 2 = McLean).</summary>
        public int ScaleVersion { get; set; } = McLeanScaleVersion;

        /// <summary>ES: Calidad de sueño (1 = insomnio/pésimo … 5 = muy reparador).</summary>
        public int Sleep { get; set; }

        /// <summary>ES: Estrés (1 = muy estresado … 5 = muy relajado).</summary>
        public int Stress { get; set; }

        /// <summary>ES: Fatiga (1 = siempre cansado … 5 = muy fresco).</summary>
        public int Fatigue { get; set; }

        /// <summary>ES: Dolor muscular (1 = muy adolorido … 5 = sin dolor).</summary>
        public int MusclePain { get; set; }

        /// <summary>ES: Estado de ánimo (1 = muy molesto/irritable … 5 = muy positivo). 0 en registros v1.</summary>
        public int Mood { get; set; }

        /// <summary>ES: Puntaje total (suma de los ítems; 5-25 en McLean).</summary>
        public int TotalScore { get; set; }

        /// <summary>ES: Promedio de los ítems.</summary>
        public double AverageScore { get; set; }

        /// <summary>ES: Semáforo de disponibilidad (Green, Yellow, Red). Null en registros v1.</summary>
        [MaxLength(10)]
        public string? ReadinessLevel { get; set; }

        /// <summary>
        /// ES: true si el total era Verde pero algún ítem fue ≤ 2 y el semáforo se degradó a Amarillo.
        /// </summary>
        public bool VetoApplied { get; set; }

        /// <summary>ES: Observaciones o comentarios del atleta.</summary>
        public string? Observations { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
