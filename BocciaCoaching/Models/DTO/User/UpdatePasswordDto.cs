using System.ComponentModel.DataAnnotations;

namespace BocciaCoaching.Models.DTO.User
{
    public class UpdatePasswordDto : IValidatableObject
    {
        public Guid UserId { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string? Email { get; set; }

        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserId == Guid.Empty && string.IsNullOrWhiteSpace(Email))
            {
                yield return new ValidationResult(
                    "Debe proporcionar el usuario o el email.",
                    new[] { nameof(UserId), nameof(Email) });
            }

            if (UserId != Guid.Empty && string.IsNullOrWhiteSpace(CurrentPassword))
            {
                yield return new ValidationResult(
                    "La contraseña actual es requerida.",
                    new[] { nameof(CurrentPassword) });
            }
        }
    }
}
