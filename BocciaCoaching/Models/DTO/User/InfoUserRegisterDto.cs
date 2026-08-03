namespace BocciaCoaching.Models.DTO.User
{
    public class InfoUserRegisterDto
    {
        public InfoUserRegisterDto()
        {
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Region = string.Empty;
            Rol = 0;
            Category = string.Empty;


        }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Region { get; set; }
        public string Password { get; set; }
        public int Rol { get; set; }
        public string? Category { get; set;}

    }
}
