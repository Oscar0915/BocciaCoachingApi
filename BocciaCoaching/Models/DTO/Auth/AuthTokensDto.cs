using System.ComponentModel.DataAnnotations;

namespace BocciaCoaching.Models.DTO.Auth;

/// <summary>
/// ES: Par de tokens emitido al iniciar sesión o al refrescar.
/// - <see cref="AccessToken"/>: se envía en cada petición como <c>Authorization: Bearer ...</c>. Vida corta.
/// - <see cref="RefreshToken"/>: solo se envía a <c>/api/v1/auth/refresh</c>. Cada uso lo rota: guarda siempre el último.
/// </summary>
public class AuthTokensDto
{
    public string TokenType { get; set; } = "Bearer";
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiresAt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiresAt { get; set; }
}

public class RefreshTokenRequestDto
{
    [Required(ErrorMessage = "El refresh token es requerido")]
    public string RefreshToken { get; set; } = string.Empty;
}

public class PasswordResetRequestDto
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = string.Empty;
}

public class PasswordResetVerifyDto
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El código es requerido")]
    public string Code { get; set; } = string.Empty;
}

/// <summary>ES: Token de un solo uso que autoriza <c>ResetPassword</c> para un email.</summary>
public class PasswordResetTokenDto
{
    public string ResetToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

/// <summary>ES: Cambio de contraseña del usuario autenticado (el usuario sale del token).</summary>
public class ChangePasswordDto
{
    [Required(ErrorMessage = "La contraseña actual es requerida")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nueva contraseña es requerida")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
    [Compare(nameof(NewPassword), ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
