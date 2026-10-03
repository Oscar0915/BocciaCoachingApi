using System.ComponentModel.DataAnnotations;
using BocciaCoaching.Utils;

namespace BocciaCoaching.Models.DTO.Auth;

public class GoogleRegisterRequestDto : FirebaseLoginRequestDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    public string Name { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public Guid Rol { get; set; } = RoleIds.Coach;
}
