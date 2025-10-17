namespace BocciaCoaching.Models.DTO.Team
{
    public class RequestUpdateTeamDto
    {
        public RequestUpdateTeamDto()
        {

        }
        public int TeamId { get; set; }
        public string? Image { get; set; }
        public bool? Bc1 { get; set; }
        public bool? Bc2 { get; set; }
        public bool? Bc3 { get; set; }
        public bool? Bc4 { get; set; }
        public string? Country { get; set;  }
        public string? Region { get; set;  }
    }
}
