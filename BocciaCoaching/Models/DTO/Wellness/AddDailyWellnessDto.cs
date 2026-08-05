using System.ComponentModel.DataAnnotations;

namespace BocciaCoaching.Models.DTO.Wellness
{
    /// <summary>
    /// ES: Datos para registrar el test diario de bienestar de un atleta.
    /// Cada indicador se puntúa de 1 a 7.
    /// </summary>
    public class AddDailyWellnessDto
    {
        [Required]
        public Guid AthleteId { get; set; }

        public Guid? TeamId { get; set; }

        /// <summary>
        /// ES: Fecha del test. Si es null, se usa la fecha actual.
        /// </summary>
        public DateTime? AssessmentDate { get; set; }

        [Range(1, 7, ErrorMessage = "El valor de Sueño debe estar entre 1 y 7")]
        public int Sleep { get; set; }

        [Range(1, 7, ErrorMessage = "El valor de Estrés debe estar entre 1 y 7")]
        public int Stress { get; set; }

        [Range(1, 7, ErrorMessage = "El valor de Fatiga debe estar entre 1 y 7")]
        public int Fatigue { get; set; }

        [Range(1, 7, ErrorMessage = "El valor de Dolor Muscular debe estar entre 1 y 7")]
        public int MusclePain { get; set; }

        public string? Observations { get; set; }
    }
}

