namespace BocciaCoaching.Models.DTO.Team
{
    public class RequestTeamDto
    {
        public string? NameTeam { get; set; }
        public string? Description { get; set; }
        public int CoachId { get; set; }
        public string? Image { get; set; }
        public bool? Bc1 { get; set; }
        public bool? Bc2 { get; set; }
        public bool? Bc3 { get; set; }
        public bool? Bc4 { get; set; }
        public bool? Pairs { get; set; }
        public bool? Teams { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
    }
}
