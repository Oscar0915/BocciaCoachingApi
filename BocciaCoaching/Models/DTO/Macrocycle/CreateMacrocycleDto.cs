namespace BocciaCoaching.Models.DTO.Macrocycle
{
    public class CreateMacrocycleDto
    {
        public int AthleteId { get; set; }
        public string AthleteName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Notes { get; set; }
        public int CoachId { get; set; }
        public int TeamId { get; set; }
        public List<CreateMacrocycleEventDto> Events { get; set; } = new();

        /// <summary>
        /// Opcional: si se proporcionan, se usan en lugar de auto-calcularlos.
        /// </summary>
        public List<CreateMesocycleDto>? Mesocycles { get; set; }

        /// <summary>
        /// Opcional: si se proporcionan, se usan en lugar de auto-calcularlos.
        /// </summary>
        public List<CreateMicrocycleDto>? Microcycles { get; set; }
    }
}

