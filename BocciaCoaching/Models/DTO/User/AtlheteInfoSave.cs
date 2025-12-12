namespace BocciaCoaching.Models.DTO.User
{
    public class AtlheteInfoSave
    {
        public string? Dni { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Address { get; set; }

        public DateTime? Seniority { get; set; }

        public bool Status { get; set; } = true;

        public int CoachId { get; set; }

        public int? TeamId { get; set; }
     
    }
}
