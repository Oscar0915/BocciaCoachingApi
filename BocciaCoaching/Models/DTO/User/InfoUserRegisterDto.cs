namespace BocciaCoaching.Models.DTO.User
{
    public class InfoUserRegisterDto
    {
        public InfoUserRegisterDto()
        {
            FirstName= string.Empty;
            Email = string.Empty;
            Password= string.Empty;
        }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
