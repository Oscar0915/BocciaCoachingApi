namespace BocciaCoaching.Models.DTO.Macrocycle
{
    public class UpdateMacrocycleEventDto
    {
        public string MacrocycleEventId { get; set; } = string.Empty;
        public string MacrocycleId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        /// <summary>competencia, concentracion, evaluacion, descanso, campus, controlTecnico, intercambio</summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>Nivel del evento (para competencias): local, nacional, internacional</summary>
        public string? Level { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
    }
}
