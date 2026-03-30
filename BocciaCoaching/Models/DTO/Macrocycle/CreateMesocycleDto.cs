namespace BocciaCoaching.Models.DTO.Macrocycle
{
    public class CreateMesocycleDto
    {
        public int Number { get; set; }
        public string Name { get; set; } = string.Empty;

        /// <summary>introductorio, desarrollador, estabilizador, competitivo, recuperacion, precompetitivo</summary>
        public string Type { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Weeks { get; set; }
        public string? Objective { get; set; }
    }
}

