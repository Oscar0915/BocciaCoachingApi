using BocciaCoaching.Models.DTO.General;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Infrastructure.Api;

/// <summary>
/// ES: Base para los controladores REST (<c>/api/v1</c>). Traduce el <see cref="ResultStatus"/>
/// de cada <see cref="ResponseContract{T}"/> al código HTTP correcto, manteniendo el mismo
/// cuerpo JSON <c>{ success, message, data }</c>.
/// </summary>
[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ResponseContract<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ResponseContract<object>), StatusCodes.Status500InternalServerError)]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>
    /// ES: Devuelve la respuesta con el código HTTP que corresponde a su <see cref="ResultStatus"/>.
    /// </summary>
    protected ActionResult<ResponseContract<T>> FromResult<T>(ResponseContract<T> result)
        => StatusCode(ToHttpStatus(result.Status), result);

    /// <summary>
    /// ES: Para operaciones de creación: 201 + cabecera Location cuando tiene éxito;
    /// en caso contrario, el código de error correspondiente.
    /// </summary>
    protected ActionResult<ResponseContract<T>> CreatedFromResult<T>(
        ResponseContract<T> result, string routeName, object routeValues)
    {
        if (!result.Success)
            return FromResult(result);

        result.Status = ResultStatus.Created;
        return CreatedAtRoute(routeName, routeValues, result);
    }

    public static int ToHttpStatus(ResultStatus status) => status switch
    {
        ResultStatus.Ok => StatusCodes.Status200OK,
        ResultStatus.Created => StatusCodes.Status201Created,
        ResultStatus.Invalid => StatusCodes.Status400BadRequest,
        ResultStatus.NotFound => StatusCodes.Status404NotFound,
        ResultStatus.Conflict => StatusCodes.Status409Conflict,
        ResultStatus.Forbidden => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
    };
}
