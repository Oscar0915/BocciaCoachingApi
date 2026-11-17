namespace BocciaCoaching.Models.DTO.General;

/// <summary>
/// ES: Resultado semántico de una operación. Los controladores lo traducen al código HTTP
/// correspondiente (ver <c>ApiControllerBase.ToHttpStatus</c>). No se serializa en la respuesta.
/// EN: Semantic outcome of an operation, mapped to an HTTP status code by the API layer.
/// </summary>
public enum ResultStatus
{
    /// <summary>200 OK</summary>
    Ok = 0,

    /// <summary>201 Created</summary>
    Created,

    /// <summary>400 Bad Request — datos de entrada inválidos o regla de negocio incumplida.</summary>
    Invalid,

    /// <summary>404 Not Found</summary>
    NotFound,

    /// <summary>409 Conflict — el recurso ya existe o el estado actual impide la operación.</summary>
    Conflict,

    /// <summary>403 Forbidden</summary>
    Forbidden,

    /// <summary>500 Internal Server Error</summary>
    Error
}
