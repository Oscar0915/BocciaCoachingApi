namespace BocciaCoaching.Models.DTO.AssessDirection
{
    public class ActiveDirectionEvaluationDto
    {
        public Guid AssessDirectionId { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string? Description { get; set; }
        public string? State { get; set; }
        public Guid TeamId { get; set; }
        public string? TeamName { get; set; }
        public Guid CreatedByCoachId { get; set; }
        public string? CreatedByCoachName { get; set; }
        public string? CreatedByCoachEmail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Lista de atletas participantes
        public List<AthleteInDirectionEvaluationDto> Athletes { get; set; } = new List<AthleteInDirectionEvaluationDto>();

        // Todos los lanzamientos de la evaluación
        public List<DirectionEvaluationThrowDto> Throws { get; set; } = new List<DirectionEvaluationThrowDto>();
    }

    public class AthleteInDirectionEvaluationDto
    {
        public Guid AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public string? AthleteEmail { get; set; }
        public Guid CoachId { get; set; }
        public string? CoachName { get; set; }
    }

    public class DirectionEvaluationThrowDto
    {
        public Guid EvaluationDetailDirectionId { get; set; }
        public int BoxNumber { get; set; }
        public int ThrowOrder { get; set; }
        public decimal? TargetDistance { get; set; }
        public decimal? ScoreObtained { get; set; }
        public string? Observations { get; set; }
        public bool Status { get; set; }
        public Guid AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public bool DeviatedRight { get; set; }
        public bool DeviatedLeft { get; set; }
        public bool IsStrength { get; set; }
        public bool IsCadence { get; set; }
        public bool IsDirection { get; set; }
        public bool IsTrajectory { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

