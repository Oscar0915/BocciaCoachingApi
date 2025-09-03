namespace BocciaCoaching.Models.DTO.User
{
    public class InfoBasicUserDto
    {

        public InfoBasicUserDto()
        {
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            UserName = string.Empty;
            LastName = string.Empty;
        }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
    }
}
