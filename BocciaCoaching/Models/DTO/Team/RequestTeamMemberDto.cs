namespace BocciaCoaching.Models.DTO.Team
{
    public class RequestTeamMemberDto
    {
        public int UserId { get; set; }
        public int TeamId { get; set; }
        public DateTime DateCreation { get; set; }

    }
}
