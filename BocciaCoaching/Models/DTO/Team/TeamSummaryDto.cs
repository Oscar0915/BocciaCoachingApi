namespace BocciaCoaching.Models.DTO.Team
{
    /// <summary>
    /// DTO que representa el resumen de un equipo incluyendo la cantidad de integrantes
    /// </summary>
    public class TeamSummaryDto
    {
        public int TeamId { get; set; }
        public string? NameTeam { get; set; }
        public string? Description { get; set; }
        public int CoachId { get; set; }
        public bool? Status { get; set; }
        public string? Image { get; set; }
        public bool? Bc1 { get; set; }
        public bool? Bc2 { get; set; }
        public bool? Bc3 { get; set; }
        public bool? Bc4 { get; set; }
        public bool? Pairs { get; set; }
        public bool? Teams { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Cantidad de integrantes del equipo
        /// </summary>
        public int MemberCount { get; set; }
    }
}

