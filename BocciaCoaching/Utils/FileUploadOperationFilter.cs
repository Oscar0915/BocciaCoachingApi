using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace BocciaCoaching.Utils
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParameters = context.MethodInfo
                .GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile) || 
                           p.ParameterType == typeof(ICollection<IFormFile>) ||
                           p.ParameterType == typeof(IEnumerable<IFormFile>) ||
                           p.GetCustomAttribute<Microsoft.AspNetCore.Mvc.FromFormAttribute>() != null)
                .ToList();

            if (fileParameters.Any())
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>(),
                                Required = new HashSet<string>()
                            }
                        }
                    }
                };

                foreach (var fileParameter in fileParameters)
                {
                    var schema = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    };

                    operation.RequestBody.Content["multipart/form-data"].Schema.Properties[fileParameter.Name!] = schema;
                    
                    // Para este caso específico, como el parámetro en UpdateMyAvatar es nullable (IFormFile? file),
                    // no lo marcamos como requerido por defecto
                }

                // Remover parámetros de archivos de la lista de parámetros
                var parametersToRemove = operation.Parameters
                    .Where(p => fileParameters.Any(fp => fp.Name == p.Name))
                    .ToList();

                foreach (var parameter in parametersToRemove)
                {
                    operation.Parameters.Remove(parameter);
                }
            }
        }
    }
}


