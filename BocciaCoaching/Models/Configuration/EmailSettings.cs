namespace BocciaCoaching.Models.Configuration
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public string FromEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public bool UseSsl { get; set; } = true;

        // Configuración de la API REST de Hostinger Mail (https://api.mail.hostinger.com)
        // Si UseApi es true, los correos se envían mediante la API en lugar de SMTP.
        public bool UseApi { get; set; } = false;
        public string ApiBaseUrl { get; set; } = "https://api.mail.hostinger.com";
        public string ApiToken { get; set; } = string.Empty;
        public string MailboxId { get; set; } = string.Empty;
    }
}

