using BocciaCoaching.Models.DTO.General;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Infrastructure.Api;

public static class ApiServiceCollectionExtensions
{
    /// <summary>
    /// ES: Unifica la respuesta de validación automática de [ApiController] con el contrato estándar:
    /// 400 + <c>{ success:false, message, data: { campo: [errores] } }</c>.
    /// Los endpoints legacy conservan la respuesta por defecto (ValidationProblemDetails).
    /// </summary>
    public static IMvcBuilder AddStandardValidationResponse(this IMvcBuilder builder)
    {
        return builder.ConfigureApiBehaviorOptions(options =>
        {
            var defaultFactory = options.InvalidModelStateResponseFactory;

            options.InvalidModelStateResponseFactory = context =>
            {
                var isLegacy = context.ActionDescriptor.EndpointMetadata.OfType<LegacyApiAttribute>().Any();
                if (isLegacy)
                    return defaultFactory(context);

                var errors = context.ModelState
                    .Where(entry => entry.Value is { Errors.Count: > 0 })
                    .ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value!.Errors
                            .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Valor inválido." : e.ErrorMessage)
                            .ToArray());

                var body = new ResponseContract<Dictionary<string, string[]>>(
                    false, "La solicitud contiene datos inválidos.", errors);

                return new BadRequestObjectResult(body);
            };
        });
    }
}
