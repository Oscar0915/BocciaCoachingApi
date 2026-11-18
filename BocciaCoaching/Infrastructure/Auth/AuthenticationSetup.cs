using System.Security.Cryptography;
using System.Threading.RateLimiting;
using BocciaCoaching.Models.DTO.General;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;

namespace BocciaCoaching.Infrastructure.Auth;

public static class AuthenticationSetup
{
    /// <summary>Política de rate limiting para login, refresh, códigos y restablecimiento.</summary>
    public const string AuthRateLimitPolicy = "auth";

    /// <summary>
    /// ES: Registra JWT Bearer, la política de autorización global y el rate limiting.
    /// - TODA petición exige un access token válido salvo los endpoints con <c>[AllowAnonymous]</c>.
    /// - 401/403 responden con el contrato estándar <c>{ success:false, message, data:null }</c>.
    /// </summary>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .PostConfigure(options =>
            {
                // En desarrollo, si no hay clave configurada, se genera una aleatoria por proceso
                // (los access tokens dejan de valer al reiniciar; los refresh tokens siguen sirviendo).
                if (string.IsNullOrWhiteSpace(options.SigningKey) && environment.IsDevelopment())
                    options.SigningKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
            })
            .Validate(options => System.Text.Encoding.UTF8.GetByteCount(options.SigningKey ?? string.Empty) >= 32,
                "Falta Jwt:SigningKey (mín. 32 bytes). Defínela con la variable de entorno Jwt__SigningKey.")
            .ValidateOnStart();

        services.AddSingleton<ITokenService, TokenService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

        // Los parámetros de validación salen de TokenService (misma clave, emisor y audiencia).
        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<ITokenService>((options, tokenService) =>
            {
                options.MapInboundClaims = false; // conserva "sub" y "role" tal cual
                options.TokenValidationParameters = tokenService.GetAccessTokenValidationParameters();
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        var expired = context.AuthenticateFailure is Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException;
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.Headers.WWWAuthenticate = expired
                            ? "Bearer error=\"invalid_token\", error_description=\"expired\""
                            : "Bearer";
                        await context.Response.WriteAsJsonAsync(new ResponseContract<object?>(false,
                            expired ? "El token expiró. Renueva la sesión." : "Debes iniciar sesión para acceder a este recurso.",
                            null));
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsJsonAsync(new ResponseContract<object?>(false,
                            "No tienes permisos para realizar esta operación.", null));
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            // Seguro por defecto: un endpoint nuevo queda protegido aunque se olvide [Authorize].
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            // Cupo por IP y por endpoint: el refresh no consume el cupo del login, y un equipo
            // que inicia sesión a la vez desde el mismo wifi (misma IP) no queda bloqueado.
            options.AddPolicy(AuthRateLimitPolicy, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    $"{httpContext.Connection.RemoteIpAddress}|{httpContext.Request.Path.Value?.ToLowerInvariant()}",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 20,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0
                    }));
            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.Headers.RetryAfter = "60";
                await context.HttpContext.Response.WriteAsJsonAsync(new ResponseContract<object?>(false,
                    "Demasiados intentos. Espera un minuto e inténtalo de nuevo.", null), cancellationToken);
            };
        });

        // Detrás del proxy de Render la IP real llega en X-Forwarded-For. ForwardLimit = 1 toma solo
        // la entrada que añade el proxy (la última), así el cliente no puede falsear su IP.
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.ForwardLimit = 1;
#if NET10_0_OR_GREATER
            options.KnownIPNetworks.Clear();
#else
            options.KnownNetworks.Clear();
#endif
            options.KnownProxies.Clear();
        });

        return services;
    }
}
