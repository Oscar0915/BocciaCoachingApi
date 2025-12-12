namespace BocciaCoaching.Models.DTO.Team
{
    /// <summary>
    /// DTO para devolver información de miembros del equipo sin datos sensibles
    /// </summary>
    public class TeamMemberDto
    {
        public int UserId { get; set; }
        public string? Dni { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? Image { get; set; }
        public string? Category { get; set; }
        public DateTime? Seniority { get; set; }
        public bool? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

