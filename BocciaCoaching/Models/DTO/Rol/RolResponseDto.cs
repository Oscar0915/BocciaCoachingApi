namespace BocciaCoaching.Models.DTO.Rol
{
    /// <summary>
    /// ES: Representación de un rol devuelta por la API.
    /// EN: Role representation returned by the API.
    /// </summary>
    public class RolResponseDto
    {
        public Guid RolId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

