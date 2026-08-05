namespace BocciaCoaching.Models.DTO.Team
{
    public class RequestTeamMemberDto
    {
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
        public DateTime DateCreation { get; set; }

    }
}
