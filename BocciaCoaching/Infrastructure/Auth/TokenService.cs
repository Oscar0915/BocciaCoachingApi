using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace BocciaCoaching.Infrastructure.Auth;

public interface ITokenService
{
    /// <summary>ES: Crea un access token JWT firmado para el usuario con sus roles.</summary>
    (string Token, DateTime ExpiresAt) CreateAccessToken(Guid userId, string? email, IEnumerable<string> roles);

    /// <summary>ES: Genera un refresh token aleatorio (valor para el cliente + hash para la BD).</summary>
    (string Token, string Hash) CreateRefreshToken();

    string HashRefreshToken(string token);

    /// <summary>ES: Token de un solo uso que autoriza restablecer la contraseña del email indicado.</summary>
    (string Token, DateTime ExpiresAt) CreatePasswordResetToken(string email);

    /// <summary>
    /// ES: Valida y consume el token de restablecimiento. Devuelve el email autorizado
    /// o null si es inválido, expiró o ya se usó.
    /// </summary>
    Task<string?> ConsumePasswordResetTokenAsync(string token);

    /// <summary>ES: Parámetros de validación de los access tokens (los usa JwtBearer).</summary>
    TokenValidationParameters GetAccessTokenValidationParameters();
}

public sealed class TokenService : ITokenService
{
    private const string PurposeClaim = "purpose";
    private const string PasswordResetPurpose = "password-reset";
    private static readonly object ConsumeLock = new();

    private readonly JwtOptions _options;
    private readonly SymmetricSecurityKey _signingKey;
    private readonly SigningCredentials _signingCredentials;
    private readonly JsonWebTokenHandler _handler = new() { MapInboundClaims = false };
    private readonly IMemoryCache _cache;

    public TokenService(IOptions<JwtOptions> options, IMemoryCache cache)
    {
        _options = options.Value;
        _cache = cache;
        _signingKey = CreateSigningKey(_options.SigningKey);
        _signingCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
    }

    public static SymmetricSecurityKey CreateSigningKey(string signingKey)
    {
        var bytes = Encoding.UTF8.GetBytes(signingKey ?? string.Empty);
        if (bytes.Length < 32)
            throw new InvalidOperationException(
                "Jwt:SigningKey debe tener al menos 32 bytes. Defínela con la variable de entorno Jwt__SigningKey.");
        return new SymmetricSecurityKey(bytes);
    }

    public (string Token, DateTime ExpiresAt) CreateAccessToken(Guid userId, string? email, IEnumerable<string> roles)
    {
        var now = DateTime.UtcNow;
        var expiresAt = now.AddMinutes(_options.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };
        if (!string.IsNullOrWhiteSpace(email))
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        claims.AddRange(roles.Distinct().Select(role => new Claim("role", role)));

        var token = _handler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            IssuedAt = now,
            NotBefore = now,
            Expires = expiresAt,
            SigningCredentials = _signingCredentials
        });

        return (token, expiresAt);
    }

    public (string Token, string Hash) CreateRefreshToken()
    {
        var token = Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(64));
        return (token, HashRefreshToken(token));
    }

    public string HashRefreshToken(string token)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token))).ToLowerInvariant();

    public (string Token, DateTime ExpiresAt) CreatePasswordResetToken(string email)
    {
        var now = DateTime.UtcNow;
        var expiresAt = now.AddMinutes(_options.PasswordResetTokenMinutes);

        var token = _handler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email.Trim().ToLowerInvariant()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(PurposeClaim, PasswordResetPurpose)
            }),
            Issuer = _options.Issuer,
            Audience = _options.PasswordResetAudience, // no sirve como access token
            IssuedAt = now,
            NotBefore = now,
            Expires = expiresAt,
            SigningCredentials = _signingCredentials
        });

        return (token, expiresAt);
    }

    public async Task<string?> ConsumePasswordResetTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        var result = await _handler.ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidIssuer = _options.Issuer,
            ValidAudience = _options.PasswordResetAudience,
            IssuerSigningKey = _signingKey,
            ValidateIssuerSigningKey = true,
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
            ClockSkew = TimeSpan.FromSeconds(30)
        });

        if (!result.IsValid ||
            !result.Claims.TryGetValue(PurposeClaim, out var purpose) || purpose as string != PasswordResetPurpose ||
            !result.Claims.TryGetValue(JwtRegisteredClaimNames.Jti, out var jtiValue) || jtiValue is not string jti ||
            !result.Claims.TryGetValue(JwtRegisteredClaimNames.Email, out var emailValue) || emailValue is not string email)
        {
            return null;
        }

        // Un solo uso: el jti queda marcado hasta que el token expire.
        // Nota: en memoria del proceso; con varias instancias habría que usar una caché distribuida.
        var usedKey = $"pwd-reset-used:{jti}";
        lock (ConsumeLock)
        {
            if (_cache.TryGetValue(usedKey, out _))
                return null;
            _cache.Set(usedKey, true, TimeSpan.FromMinutes(_options.PasswordResetTokenMinutes + 1));
        }

        return email;
    }

    public TokenValidationParameters GetAccessTokenValidationParameters() => new()
    {
        ValidIssuer = _options.Issuer,
        ValidAudience = _options.Audience,
        IssuerSigningKey = _signingKey,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
        NameClaimType = JwtRegisteredClaimNames.Sub,
        RoleClaimType = "role",
        ClockSkew = TimeSpan.FromSeconds(30)
    };
}
