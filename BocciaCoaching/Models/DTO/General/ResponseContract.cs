namespace BocciaCoaching.Models.DTO.General;

public class ResponseContract<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public ResponseContract(string message, T data)
    {
        Message = message;
        Data = data;
    }

    public ResponseContract(bool success, string message, T data)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    // Factory helpers
    public static ResponseContract<T> Ok(T data, string message = "Operación exitosa")
    {
        return new ResponseContract<T>(true, message, data);
    }

    public static ResponseContract<T?> Fail(string message = "Error en la operación")
    {
        return new ResponseContract<T?>(false, message, default(T));
    }
}