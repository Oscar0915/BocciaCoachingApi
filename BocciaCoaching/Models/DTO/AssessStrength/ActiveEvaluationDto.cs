namespace BocciaCoaching.Models.DTO.AssessStrength
{
    public class ActiveEvaluationDto
    {
        public Guid AssessStrengthId { get; set; }
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
        public List<AthleteInEvaluationDto> Athletes { get; set; } = new List<AthleteInEvaluationDto>();
        
        // Todos los lanzamientos de la evaluación
        public List<EvaluationThrowDto> Throws { get; set; } = new List<EvaluationThrowDto>();
    }

    public class AthleteInEvaluationDto
    {
        public Guid AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public string? AthleteEmail { get; set; }
        public Guid CoachId { get; set; }
        public string? CoachName { get; set; }
    }

    public class EvaluationThrowDto
    {
        public Guid EvaluationDetailStrengthId { get; set; }
        public int BoxNumber { get; set; }
        public int ThrowOrder { get; set; }
        public decimal? TargetDistance { get; set; }
        public decimal? ScoreObtained { get; set; }
        public string? Observations { get; set; }
        public bool Status { get; set; }
        public Guid AthleteId { get; set; }
        public string? AthleteName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
