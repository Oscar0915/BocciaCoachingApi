using BocciaCoaching.Models.DTO.General;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Infrastructure.Api;

/// <summary>
/// ES: Manejo centralizado de excepciones no controladas.
/// - Registra el error con ILogger (en lugar de Console.WriteLine).
/// - No expone detalles internos (ex.Message) fuera de Development.
/// - Responde siempre con el contrato estándar <c>{ success:false, message, data:null }</c>.
/// - Endpoints REST: código HTTP real (400/404/409/500). Endpoints legacy: HTTP 200.
/// - Agrega la cabecera <c>X-Trace-Id</c> para poder correlacionar con los logs.
/// </summary>
public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment) : IExceptionHandler
{
    private const string GenericErrorMessage = "Ocurrió un error inesperado. Intenta nuevamente.";

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // El cliente canceló la petición: no es un error del servidor.
        if (exception is OperationCanceledException && httpContext.RequestAborted.IsCancellationRequested)
        {
            httpContext.Response.StatusCode = 499; // Client Closed Request
            return true;
        }

        var (statusCode, message) = exception switch
        {
            ArgumentException or FormatException => (StatusCodes.Status400BadRequest, exception.Message),
            KeyNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            DbUpdateConcurrencyException => (StatusCodes.Status409Conflict,
                "El registro fue modificado por otro proceso. Vuelve a cargarlo e intenta de nuevo."),
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden,
                "No tienes permisos para realizar esta operación."),
            _ => (StatusCodes.Status500InternalServerError, GenericErrorMessage)
        };

        if (statusCode >= StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Error no controlado en {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);
        else
            logger.LogWarning(exception, "Petición rechazada en {Method} {Path}: {Message}", httpContext.Request.Method, httpContext.Request.Path, exception.Message);

        if (statusCode == StatusCodes.Status500InternalServerError && environment.IsDevelopment())
            message = $"{GenericErrorMessage} [{exception.GetType().Name}: {exception.Message}]";

        // UseExceptionHandler limpia el endpoint del HttpContext; el original queda en el feature.
        var endpoint = httpContext.Features.Get<IExceptionHandlerFeature>()?.Endpoint ?? httpContext.GetEndpoint();
        var isLegacy = endpoint?.Metadata.GetMetadata<LegacyApiAttribute>() is not null;

        httpContext.Response.StatusCode = isLegacy ? StatusCodes.Status200OK : statusCode;
        httpContext.Response.Headers["X-Trace-Id"] = httpContext.TraceIdentifier;

        var body = new ResponseContract<object?>(false, message, null);
        await httpContext.Response.WriteAsJsonAsync(body, cancellationToken);
        return true;
    }
}
