using BocciaCoaching.Models.DTO.Statistic;

namespace BocciaCoaching.Models.DTO.AssessStrength
{
    /// <summary>
    /// Detalles completos de una evaluación de fuerza
    /// </summary>
    public class EvaluationDetailsDto
    {
        public int AssessStrengthId { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string? Description { get; set; }
        public string? State { get; set; }
        public string? StateName { get; set; }
        public int TeamId { get; set; }
        public string? TeamName { get; set; }
        public int CoachId { get; set; }
        public string? CoachName { get; set; }
        public string? CoachEmail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Lista de atletas participantes
        public List<AthleteInEvaluationDto> Athletes { get; set; } = new List<AthleteInEvaluationDto>();
        
        // Todos los lanzamientos de la evaluación
        public List<EvaluationThrowDto> Throws { get; set; } = new List<EvaluationThrowDto>();
        
        // Estadísticas de los atletas (si la evaluación está terminada)
        public List<AthleteStatisticsDto>? Statistics { get; set; }
    }
}

