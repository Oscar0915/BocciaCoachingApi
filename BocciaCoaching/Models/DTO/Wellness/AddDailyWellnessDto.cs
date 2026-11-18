using System.ComponentModel.DataAnnotations;

namespace BocciaCoaching.Models.DTO.Wellness
{
    /// <summary>
    /// ES: Respuestas del cuestionario McLean. Cada ítem va de 1 (pésimo) a 5 (excelente).
    /// </summary>
    public class WellnessAnswersDto
    {
        /// <summary>Fatiga: 1 = siempre cansado … 5 = muy fresco.</summary>
        [Range(1, 5, ErrorMessage = "La fatiga debe estar entre 1 y 5")]
        public int Fatigue { get; set; }

        /// <summary>Calidad de sueño: 1 = insomnio/pésimo … 5 = muy reparador.</summary>
        [Range(1, 5, ErrorMessage = "La calidad de sueño debe estar entre 1 y 5")]
        public int Sleep { get; set; }

        /// <summary>Dolor muscular: 1 = muy adolorido … 5 = sin dolor.</summary>
        [Range(1, 5, ErrorMessage = "El dolor muscular debe estar entre 1 y 5")]
        public int MusclePain { get; set; }

        /// <summary>Estrés: 1 = muy estresado … 5 = muy relajado.</summary>
        [Range(1, 5, ErrorMessage = "El nivel de estrés debe estar entre 1 y 5")]
        public int Stress { get; set; }

        /// <summary>Estado de ánimo: 1 = muy molesto/irritable … 5 = muy positivo.</summary>
        [Range(1, 5, ErrorMessage = "El estado de ánimo debe estar entre 1 y 5")]
        public int Mood { get; set; }

        [MaxLength(1000, ErrorMessage = "Las observaciones no pueden superar 1000 caracteres")]
        public string? Observations { get; set; }
    }

    /// <summary>
    /// ES: El atleta autenticado registra su cuestionario de hoy (<c>POST /api/v1/wellness/me</c>).
    /// La fecha la pone el servidor (día local del atleta).
    /// </summary>
    public class SubmitMyWellnessDto : WellnessAnswersDto
    {
        /// <summary>Equipo en cuyo contexto responde (opcional).</summary>
        public Guid? TeamId { get; set; }
    }

    /// <summary>
    /// ES: Registro del cuestionario para un atleta concreto. Un atleta solo puede registrar el suyo;
    /// un entrenador puede registrarlo por un atleta de sus equipos (p. ej. si el atleta necesita
    /// asistencia) y elegir un día pasado; un admin, para cualquiera.
    /// </summary>
    public class AddDailyWellnessDto : WellnessAnswersDto
    {
        [Required]
        public Guid AthleteId { get; set; }

        public Guid? TeamId { get; set; }

        /// <summary>
        /// ES: Día del cuestionario. Si es null se usa el día de hoy del atleta. Solo entrenador o admin
        /// pueden indicar otro día, y nunca uno futuro.
        /// </summary>
        public DateTime? AssessmentDate { get; set; }
    }
}
