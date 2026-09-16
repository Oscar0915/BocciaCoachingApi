using System.ComponentModel.DataAnnotations;

namespace BocciaCoaching.Models.DTO.Auth;

public class FirebaseLoginRequestDto
{
    [Required(ErrorMessage = "El token de Firebase es requerido")]
    public string IdToken { get; set; } = string.Empty;
}
