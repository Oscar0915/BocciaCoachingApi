using System.Security.Claims;
using BocciaCoaching.Infrastructure.Auth;
using Microsoft.AspNetCore.Http;

namespace BocciaCoaching.Services.Wellness
{
    /// <summary>
    /// ES: Configuración del cuestionario (sección "Wellness").
    /// </summary>
    public sealed class WellnessOptions
    {
        public const string SectionName = "Wellness";

        /// <summary>
        /// ES: Zona horaria (IANA) que define "hoy" cuando la app no envía la cabecera <c>X-Time-Zone</c>.
        /// El servidor corre en UTC; sin esto, a las 7 p. m. en Colombia ya sería "mañana".
        /// </summary>
        public string DefaultTimeZone { get; set; } = "America/Bogota";
    }

    /// <summary>
    /// ES: Quién hace la petición y en qué zona horaria está. Se construye desde el token y la cabecera
    /// <c>X-Time-Zone</c>; el servicio lo usa para permisos y para calcular "hoy".
    /// </summary>
    public sealed record WellnessRequester(Guid UserId, bool IsAdmin, bool IsCoach, bool IsAthlete, TimeZoneInfo TimeZone)
    {
        public const string TimeZoneHeader = "X-Time-Zone";

        public static WellnessRequester From(ClaimsPrincipal user, HttpRequest request, WellnessOptions options)
            => new(
                user.GetUserId(),
                user.IsInRole(AuthRoles.Admin),
                user.IsInRole(AuthRoles.Coach),
                user.IsInRole(AuthRoles.Athlete),
                ResolveTimeZone(request.Headers[TimeZoneHeader].ToString(), options.DefaultTimeZone));

        /// <summary>ES: Zona de la cabecera si es válida; si no, la configurada; si tampoco, UTC.</summary>
        public static TimeZoneInfo ResolveTimeZone(string? requested, string fallback)
        {
            foreach (var id in new[] { requested, fallback })
            {
                if (string.IsNullOrWhiteSpace(id) || id.Length > 64)
                    continue;
                if (TimeZoneInfo.TryFindSystemTimeZoneById(id.Trim(), out var zone))
                    return zone;
            }
            return TimeZoneInfo.Utc;
        }
    }
}
