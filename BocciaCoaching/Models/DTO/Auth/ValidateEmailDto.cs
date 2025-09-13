namespace BocciaCoaching.Models.DTO.Auth
{
    public class ValidateEmailDto
    {
        public ValidateEmailDto()
        {
            Email= string.Empty;
        }
        public string Email { get; set; }
    }
}
