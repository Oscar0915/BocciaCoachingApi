using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("SaremasThrow")]
    public class SaremasThrow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SaremasThrowId { get; set; }

        public int SaremasEvalId { get; set; }

        [ForeignKey("SaremasEvalId")]
        public SaremasEvaluation? SaremasEvaluation { get; set; }

        public int AthleteId { get; set; }

        [ForeignKey("AthleteId")]
        public User? Athlete { get; set; }

        /// <summary>Número del tiro (1–28)</summary>
        public int ThrowNumber { get; set; }

        /// <summary>Roja o Azul</summary>
        public string Diagonal { get; set; } = string.Empty;

        /// <summary>Componente técnico (Salida, Romper, Arrimar, etc.)</summary>
        public string TechnicalComponent { get; set; } = string.Empty;

        /// <summary>Puntaje (0–5)</summary>
        public int ScoreObtained { get; set; }

        public string? Observations { get; set; }

        /// <summary>Tags separados por coma: Fuerza,Dirección,Cadencia,Trayectoria</summary>
        public string? FailureTags { get; set; }

        public bool Status { get; set; } = true;

        // Datos de cancha (solo para componente "Salida")
        public double? WhiteBallX { get; set; }
        public double? WhiteBallY { get; set; }
        public double? ColorBallX { get; set; }
        public double? ColorBallY { get; set; }
        public double? EstimatedDistance { get; set; }
        public double? LaunchPointX { get; set; }
        public double? LaunchPointY { get; set; }
        public double? DistanceToLaunchPoint { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

