namespace BocciaCoaching.Models.DTO.AssessDirection
{
    /// <summary>
    /// Detalles completos de una evaluación de control de dirección
    /// </summary>
    public class DirectionEvaluationDetailsDto
    {
        public int AssessDirectionId { get; set; }
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
        public List<AthleteInDirectionEvaluationDto> Athletes { get; set; } = new List<AthleteInDirectionEvaluationDto>();

        // Todos los lanzamientos de la evaluación
        public List<DirectionEvaluationThrowDto> Throws { get; set; } = new List<DirectionEvaluationThrowDto>();

        // Estadísticas de los atletas (si la evaluación está terminada)
        public List<DirectionAthleteStatisticsDto>? Statistics { get; set; }
    }
}

