using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BocciaCoaching.Infrastructure.Auth;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// ES: Identificador del usuario autenticado (claim "sub" del access token).
    /// Lanza si no hay usuario: solo debe usarse en endpoints protegidos.
    /// </summary>
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return Guid.TryParse(value, out var userId)
            ? userId
            : throw new UnauthorizedAccessException("El token no identifica a un usuario.");
    }

    public static bool IsAdmin(this ClaimsPrincipal user) => user.IsInRole(AuthRoles.Admin);
}
