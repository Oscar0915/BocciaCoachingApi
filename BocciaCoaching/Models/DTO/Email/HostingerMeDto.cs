using System.Text.Json.Serialization;

namespace BocciaCoaching.Models.DTO.Email
{
    /// <summary>
    /// Respuesta del endpoint de autorización de la API de Hostinger Mail:
    /// GET {ApiBaseUrl}/api/v1/me
    /// Se utiliza para validar el token Bearer y obtener los buzones (mailboxes)
    /// disponibles cuando un envío falla.
    /// </summary>
    public class HostingerMeResponse
    {
        [JsonPropertyName("data")]
        public HostingerMeData? Data { get; set; }
    }

    public class HostingerMeData
    {
        [JsonPropertyName("orderResourceId")]
        public string? OrderResourceId { get; set; }

        [JsonPropertyName("mailboxes")]
        public List<HostingerMailbox> Mailboxes { get; set; } = new();
    }

    public class HostingerMailbox
    {
        [JsonPropertyName("resourceId")]
        public string? ResourceId { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }
    }
}

