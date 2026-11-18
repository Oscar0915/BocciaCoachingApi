using System.ComponentModel.DataAnnotations;

namespace BocciaCoaching.Models.DTO.User
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// ES: Token de un solo uso que devuelve <c>Email/ValidateCode</c> (o <c>/api/v1/auth/password-reset/verify</c>)
        /// al validar el código enviado al correo. Sin él no se puede restablecer la contraseña.
        /// </summary>
        [Required(ErrorMessage = "El token de restablecimiento es requerido")]
        public string ResetToken { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
