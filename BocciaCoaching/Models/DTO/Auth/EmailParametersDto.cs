namespace BocciaCoaching.Models.DTO.Auth
{
    public class EmailParametersDto
    {
        public string ToEmail { get; set; }
        public string Code { get; set; }
        public int MinutesValid { get; set; }
    }
}
