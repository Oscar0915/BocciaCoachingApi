namespace BocciaCoaching.Infrastructure.Api;

/// <summary>
/// ES: Marca un controlador o acción como parte de la API antigua (rutas estilo RPC).
/// Estos endpoints conservan su comportamiento histórico: responden siempre HTTP 200
/// (también ante errores no controlados) con <c>success = false</c> en el cuerpo, para no
/// romper el frontend mientras migra a las rutas REST de <c>/api/v1</c>.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public sealed class LegacyApiAttribute : Attribute
{
}
