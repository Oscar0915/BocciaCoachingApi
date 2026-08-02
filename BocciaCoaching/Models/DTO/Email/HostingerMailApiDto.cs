using System.Text.Json.Serialization;

namespace BocciaCoaching.Models.DTO.Email
{
    /// <summary>
    /// Cuerpo de la solicitud para el endpoint de envío de la API de Hostinger Mail:
    /// POST {ApiBaseUrl}/api/v1/mailboxes/{mailboxId}/send
    ///
    /// Parámetros aceptados por la API:
    /// {
    ///   "to": ["destinatario@dominio.com"],
    ///   "displayName": "Boccia coaching",
    ///   "subject": "Hello",
    ///   "text": "Plain body",
    ///   "html": "&lt;p&gt;HTML body&lt;/p&gt;"
    /// }
    /// </summary>
    public class HostingerSendEmailRequest
    {
        [JsonPropertyName("to")]
        public List<string> To { get; set; } = new();

        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("html")]
        public string? Html { get; set; }
    }
}

