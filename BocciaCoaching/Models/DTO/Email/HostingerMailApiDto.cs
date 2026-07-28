using System.Text.Json.Serialization;

namespace BocciaCoaching.Models.DTO.Email
{
    /// <summary>
    /// Cuerpo de la solicitud para el endpoint de envío de la API de Hostinger Mail:
    /// POST {ApiBaseUrl}/api/v1/mailboxes/{mailboxId}/send
    /// </summary>
    public class HostingerSendEmailRequest
    {
        [JsonPropertyName("to")]
        public List<string> To { get; set; } = new();

        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("cc")]
        public List<string>? Cc { get; set; }

        [JsonPropertyName("bcc")]
        public List<string>? Bcc { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("html")]
        public string? Html { get; set; }

        [JsonPropertyName("attachments")]
        public List<HostingerMailAttachment>? Attachments { get; set; }

        [JsonPropertyName("inReplyTo")]
        public HostingerMailMessageRef? InReplyTo { get; set; }

        [JsonPropertyName("forwardOf")]
        public HostingerMailMessageRef? ForwardOf { get; set; }
    }

    public class HostingerMailAttachment
    {
        [JsonPropertyName("filename")]
        public string Filename { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("contentType")]
        public string? ContentType { get; set; }

        [JsonPropertyName("cid")]
        public string? Cid { get; set; }

        [JsonPropertyName("encoding")]
        public string? Encoding { get; set; } = "base64";
    }

    public class HostingerMailMessageRef
    {
        [JsonPropertyName("uid")]
        public long Uid { get; set; }

        [JsonPropertyName("folder")]
        public string Folder { get; set; } = "INBOX";
    }
}

