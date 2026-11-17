using System.Text.Json.Serialization;

namespace BocciaCoaching.Models.DTO.General;

/// <summary>
/// ES: Envoltorio estándar de todas las respuestas de la API: <c>{ success, message, data }</c>.
/// <see cref="Status"/> indica el resultado semántico y se usa para elegir el código HTTP;
/// no forma parte del JSON, así que el contrato que recibe el frontend no cambia.
/// </summary>
public class ResponseContract<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    /// <summary>
    /// ES: Resultado semántico de la operación (se traduce a código HTTP; no se serializa).
    /// </summary>
    [JsonIgnore]
    public ResultStatus Status { get; set; }

    public ResponseContract(string message, T data)
    {
        Message = message;
        Data = data;
        Status = ResultStatus.Invalid;
    }

    public ResponseContract(bool success, string message, T data)
    {
        Success = success;
        Message = message;
        Data = data;
        Status = success ? ResultStatus.Ok : ResultStatus.Invalid;
    }

    private ResponseContract(bool success, string message, T data, ResultStatus status)
        : this(success, message, data)
    {
        Status = status;
    }

    // ---------- Éxito ----------

    public static ResponseContract<T> Ok(T data, string message = "Operación exitosa")
        => new(true, message, data, ResultStatus.Ok);

    public static ResponseContract<T> Created(T data, string message = "Recurso creado correctamente")
        => new(true, message, data, ResultStatus.Created);

    // ---------- Error ----------

    /// <summary>
    /// ES: Error genérico de validación / regla de negocio (HTTP 400).
    /// Se mantiene por compatibilidad con el código existente.
    /// </summary>
    public static ResponseContract<T> Fail(string message = "Error en la operación")
        => new(false, message, default!, ResultStatus.Invalid);

    public static ResponseContract<T> Invalid(string message)
        => new(false, message, default!, ResultStatus.Invalid);

    public static ResponseContract<T> NotFound(string message = "Recurso no encontrado")
        => new(false, message, default!, ResultStatus.NotFound);

    public static ResponseContract<T> Conflict(string message)
        => new(false, message, default!, ResultStatus.Conflict);

    public static ResponseContract<T> Forbidden(string message = "No tienes permisos para realizar esta operación")
        => new(false, message, default!, ResultStatus.Forbidden);

    public static ResponseContract<T> Error(string message = "Ocurrió un error inesperado")
        => new(false, message, default!, ResultStatus.Error);
}
