namespace BocciaCoaching.Infrastructure.Auth;

/// <summary>
/// ES: Configuración de los tokens (sección "Jwt" de la configuración).
/// La clave de firma NUNCA va en appsettings.json: se define con la variable de entorno
/// <c>Jwt__SigningKey</c> (mínimo 32 bytes aleatorios, p. ej. <c>openssl rand -base64 48</c>).
/// </summary>
public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    /// <summary>Clave HMAC-SHA256 para firmar los tokens. Mínimo 32 bytes (UTF-8).</summary>
    public string SigningKey { get; set; } = string.Empty;

    public string Issuer { get; set; } = "BocciaCoaching.Api";

    /// <summary>Audiencia de los access tokens (los consume el frontend).</summary>
    public string Audience { get; set; } = "BocciaCoaching.Client";

    /// <summary>Audiencia exclusiva de los tokens de restablecimiento de contraseña.</summary>
    public string PasswordResetAudience { get; set; } = "BocciaCoaching.PasswordReset";

    /// <summary>Vida del access token. Corto: si se filtra, sirve poco tiempo.</summary>
    public int AccessTokenMinutes { get; set; } = 15;

    /// <summary>Vida del refresh token (cada uso lo rota y reinicia el plazo).</summary>
    public int RefreshTokenDays { get; set; } = 14;

    /// <summary>Vida del token de restablecimiento de contraseña (de un solo uso).</summary>
    public int PasswordResetTokenMinutes { get; set; } = 10;

    /// <summary>
    /// Ventana en la que reutilizar un refresh token recién rotado se considera una carrera
    /// del cliente (p. ej. dos pestañas) y no un robo.
    /// </summary>
    public int RefreshReuseGraceSeconds { get; set; } = 30;
}
