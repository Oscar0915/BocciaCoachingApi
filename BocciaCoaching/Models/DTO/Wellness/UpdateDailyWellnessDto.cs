using System.ComponentModel.DataAnnotations;

namespace BocciaCoaching.Models.DTO.Wellness
{
    /// <summary>
    /// ES: Corrección de un cuestionario. El atleta solo puede corregir el de hoy; el entrenador
    /// de sus atletas o un admin, cualquiera. Se recalculan el puntaje y el semáforo.
    /// </summary>
    public class UpdateDailyWellnessDto : WellnessAnswersDto
    {
        [Required]
        public Guid DailyWellnessId { get; set; }
    }
}
