namespace BocciaCoaching.Models.DTO.User
{
    public class InfoUserRegisterDto
    {
        public InfoUserRegisterDto()
        {
            Email = string.Empty;
            Password = string.Empty;
            Region = string.Empty;
            Rol = 0;
            Category = string.Empty;

        }
        public string Email { get; set; }
        public string Region { get; set; }
        public string Password { get; set; }
        public int Rol { get; set; }
        public string? Category { get; set;}
    }
}
