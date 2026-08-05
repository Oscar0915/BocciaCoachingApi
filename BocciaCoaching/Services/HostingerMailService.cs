using BocciaCoaching.Utils;
using BocciaCoaching.Data;
using BocciaCoaching.Models.Configuration;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.Email;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories;
using BocciaCoaching.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Reflection;
using System.Text.Json;

namespace BocciaCoaching.Services
{
    /// <summary>
    /// Implementación de <see cref="IEmailService"/> que envía correos a través de la
    /// API REST de Hostinger Mail (https://api.mail.hostinger.com) usando autenticación
    /// por token Bearer, en lugar de SMTP.
    /// </summary>
    public class HostingerMailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly EmailSettings _emailSettings;
        private readonly IMemoryCache _cache;
        private readonly LogErrorRepository _logErrorRepository;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public HostingerMailService(
            HttpClient httpClient,
            IMemoryCache cache,
            ApplicationDbContext context,
            IOptions<EmailSettings> emailSettings)
        {
            _httpClient = httpClient;
            _cache = cache;
            _emailSettings = emailSettings.Value;
            _logErrorRepository = new LogErrorRepository(context);
        }

        public async Task SendSecurityCodeAsync(EmailParametersDto emailParametersDto)
        {
            try
            {
                var request = new HostingerSendEmailRequest
                {
                    To = new List<string> { emailParametersDto.ToEmail },
                    DisplayName = _emailSettings.FromName,
                    Subject = "Código de verificación",
                    Text = $"Tu código de verificación es: {emailParametersDto.Code}\nEste código expira en 5 minutos."
                };

                await SendViaApiAsync(request);

                await LogErrorAsync("Sin error", MethodBase.GetCurrentMethod()?.Name ?? "SendSecurityCodeAsync");
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex.Message, MethodBase.GetCurrentMethod()?.Name ?? "SendSecurityCodeAsync");
                throw;
            }
        }

        public async Task<bool> SendEmailNotificationAsync(EmailNotificationDto emailNotification)
        {
            try
            {
                var request = new HostingerSendEmailRequest
                {
                    To = new List<string> { emailNotification.ToEmail },
                    DisplayName = string.IsNullOrWhiteSpace(emailNotification.ToName)
                        ? _emailSettings.FromName
                        : emailNotification.ToName,
                    Subject = emailNotification.Subject,
                    Text = emailNotification.PlainTextBody,
                    Html = emailNotification.IsHtml ? emailNotification.HtmlBody : null
                };

                await SendViaApiAsync(request);
                return true;
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Error enviando email general (API): {ex.Message}", "SendEmailNotificationAsync");
                return false;
            }
        }

        public async Task<bool> SendTeamInvitationEmailAsync(TeamInvitationEmailDto invitation)
        {
            try
            {
                var htmlBody = $@"
                    <html>
                    <body>
                        <h2>¡Has sido invitado a un equipo!</h2>
                        <p>Hola <strong>{invitation.AthleteName}</strong>,</p>
                        <p>El entrenador <strong>{invitation.CoachName}</strong> te ha invitado a unirte al equipo <strong>{invitation.TeamName}</strong>.</p>
                        <p>Para aceptar la invitación, haz clic en el siguiente enlace:</p>
                        <p><a href=""{invitation.InvitationLink}"" style=""background-color: #4CAF50; color: white; padding: 14px 20px; text-align: center; text-decoration: none; display: inline-block; border-radius: 4px;"">Aceptar Invitación</a></p>
                        <p>Si no puedes hacer clic en el enlace, copia y pega la siguiente URL en tu navegador:</p>
                        <p>{invitation.InvitationLink}</p>
                        <br>
                        <p>¡Bienvenido al equipo!</p>
                        <p><em>Equipo Boccia Coaching</em></p>
                    </body>
                    </html>";

                var plainTextBody = $@"
                    ¡Has sido invitado a un equipo!

                    Hola {invitation.AthleteName},

                    El entrenador {invitation.CoachName} te ha invitado a unirte al equipo {invitation.TeamName}.

                    Para aceptar la invitación, visita el siguiente enlace:
                    {invitation.InvitationLink}

                    ¡Bienvenido al equipo!

                    Equipo Boccia Coaching";

                var request = new HostingerSendEmailRequest
                {
                    To = new List<string> { invitation.ToEmail },
                    DisplayName = invitation.AthleteName,
                    Subject = $"Invitación al equipo {invitation.TeamName}",
                    Text = plainTextBody,
                    Html = htmlBody
                };

                await SendViaApiAsync(request);
                return true;
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Error enviando invitación de equipo (API): {ex.Message}", "SendTeamInvitationEmailAsync");
                return false;
            }
        }

        public async Task<bool> SendGeneralNotificationEmailAsync(GeneralNotificationEmailDto notification)
        {
            try
            {
                var htmlBody = $@"
                    <html>
                    <body>
                        <h2>{notification.NotificationTitle}</h2>
                        <p>Hola <strong>{notification.RecipientName}</strong>,</p>
                        <div style=""background-color: #f9f9f9; padding: 15px; border-left: 4px solid #4CAF50; margin: 20px 0;"">
                            <p><strong>Tipo:</strong> {notification.NotificationType}</p>
                            <p>{notification.NotificationMessage}</p>
                        </div>
                        <p>Puedes revisar más detalles en tu aplicación Boccia Coaching.</p>
                        <br>
                        <p><em>Equipo Boccia Coaching</em></p>
                    </body>
                    </html>";

                var plainTextBody = $@"
                    {notification.NotificationTitle}

                    Hola {notification.RecipientName},

                    Tipo: {notification.NotificationType}

                    {notification.NotificationMessage}

                    Puedes revisar más detalles en tu aplicación Boccia Coaching.

                    Equipo Boccia Coaching";

                var request = new HostingerSendEmailRequest
                {
                    To = new List<string> { notification.ToEmail },
                    DisplayName = notification.RecipientName,
                    Subject = notification.NotificationTitle,
                    Text = plainTextBody,
                    Html = htmlBody
                };

                await SendViaApiAsync(request);
                return true;
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Error enviando notificación general (API): {ex.Message}", "SendGeneralNotificationEmailAsync");
                return false;
            }
        }

        /// <summary>
        /// Envía la solicitud al endpoint de la API de Hostinger Mail:
        /// POST {ApiBaseUrl}/api/v1/mailboxes/{mailboxId}/send
        /// El token Bearer se configura globalmente en el HttpClient (ver Program.cs).
        /// Si el envío falla, se consulta la autorización (GET /api/v1/me) para validar
        /// el token Bearer y, si es posible, auto-corregir el MailboxId y reintentar.
        /// </summary>
        private async Task SendViaApiAsync(HostingerSendEmailRequest request, bool allowRetry = true)
        {
            if (string.IsNullOrWhiteSpace(_emailSettings.MailboxId))
                throw new InvalidOperationException("EmailSettings.MailboxId no está configurado para la API de Hostinger Mail.");

            var relativeUrl = $"/api/v1/mailboxes/{_emailSettings.MailboxId}/send";

            Console.WriteLine($"📮 Enviando email vía API Hostinger: {_emailSettings.ApiBaseUrl}{relativeUrl}");

            using var response = await _httpClient.PostAsJsonAsync(relativeUrl, request, JsonOptions);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"✅ Email enviado vía API Hostinger. Respuesta: {responseBody}");
                return;
            }

            Console.WriteLine($"❌ La API de Hostinger devolvió {(int)response.StatusCode}: {responseBody}");

            // El envío falló: consultamos la autorización para diagnosticar el problema.
            var authInfo = await ConsultAuthorizationAsync();

            // Si el token es válido pero el buzón configurado no coincide con ninguno de
            // los buzones autorizados, intentamos auto-corregir el MailboxId y reintentar.
            if (allowRetry && authInfo.IsAuthorized && authInfo.Mailboxes.Count > 0)
            {
                var matchingMailbox = ResolveMailbox(authInfo.Mailboxes);
                if (matchingMailbox != null &&
                    !string.Equals(matchingMailbox.ResourceId, _emailSettings.MailboxId, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(
                        $"🔁 MailboxId corregido de '{_emailSettings.MailboxId}' a '{matchingMailbox.ResourceId}' " +
                        $"({matchingMailbox.Address}). Reintentando envío...");

                    await LogErrorAsync(
                        $"MailboxId corregido de '{_emailSettings.MailboxId}' a '{matchingMailbox.ResourceId}' tras fallo de envío.",
                        "SendViaApiAsync");

                    _emailSettings.MailboxId = matchingMailbox.ResourceId!;
                    await SendViaApiAsync(request, allowRetry: false);
                    return;
                }
            }

            var authDetail = authInfo.IsAuthorized
                ? $"El token Bearer es válido. Buzones autorizados: {authInfo.Describe()}."
                : $"El token Bearer NO es válido o no está autorizado (consulta a /api/v1/me: {authInfo.ErrorMessage}).";

            throw new HttpRequestException(
                $"La API de Hostinger Mail respondió {(int)response.StatusCode} ({response.StatusCode}): {responseBody}. {authDetail}");
        }

        /// <summary>
        /// Consulta la autorización contra la API de Hostinger Mail:
        /// GET {ApiBaseUrl}/api/v1/me
        /// Valida el token Bearer y devuelve los buzones (mailboxes) autorizados.
        /// Se invoca automáticamente cuando un envío falla.
        /// </summary>
        private async Task<HostingerAuthResult> ConsultAuthorizationAsync()
        {
            const string relativeUrl = "/api/v1/me";
            Console.WriteLine($"🔐 Consultando autorización: {_emailSettings.ApiBaseUrl}{relativeUrl}");

            try
            {
                using var response = await _httpClient.GetAsync(relativeUrl);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var msg = $"HTTP {(int)response.StatusCode} ({response.StatusCode}): {body}";
                    Console.WriteLine($"❌ Autorización no válida. {msg}");
                    await LogErrorAsync($"Autorización de Hostinger no válida: {msg}", "ConsultAuthorizationAsync");
                    return new HostingerAuthResult { IsAuthorized = false, ErrorMessage = msg };
                }

                var me = JsonSerializer.Deserialize<HostingerMeResponse>(body, JsonOptions);
                var mailboxes = me?.Data?.Mailboxes ?? new List<HostingerMailbox>();

                Console.WriteLine($"✅ Autorización válida. Buzones: {string.Join(", ", mailboxes.Select(m => m.Address))}");

                return new HostingerAuthResult
                {
                    IsAuthorized = true,
                    Mailboxes = mailboxes
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error consultando autorización: {ex.Message}");
                await LogErrorAsync($"Error consultando autorización de Hostinger: {ex.Message}", "ConsultAuthorizationAsync");
                return new HostingerAuthResult { IsAuthorized = false, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Selecciona el buzón adecuado entre los autorizados: prioriza el que coincide con
        /// el remitente configurado (FromEmail); si no hay coincidencia, usa el primero.
        /// </summary>
        private HostingerMailbox? ResolveMailbox(List<HostingerMailbox> mailboxes)
        {
            var match = mailboxes.FirstOrDefault(m =>
                !string.IsNullOrWhiteSpace(m.ResourceId) &&
                string.Equals(m.Address, _emailSettings.FromEmail, StringComparison.OrdinalIgnoreCase));

            return match ?? mailboxes.FirstOrDefault(m => !string.IsNullOrWhiteSpace(m.ResourceId));
        }

        /// <summary>
        /// Resultado de la consulta de autorización (GET /api/v1/me).
        /// </summary>
        private sealed class HostingerAuthResult
        {
            public bool IsAuthorized { get; set; }
            public string? ErrorMessage { get; set; }
            public List<HostingerMailbox> Mailboxes { get; set; } = new();

            public string Describe() =>
                Mailboxes.Count == 0
                    ? "ninguno"
                    : string.Join(", ", Mailboxes.Select(m => $"{m.Address} ({m.ResourceId})"));
        }

        public void SaveCode(EmailParametersDto emailParametersDto)
        {
            emailParametersDto.MinutesValid = 5;
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(emailParametersDto.MinutesValid));

            _cache.Set(emailParametersDto.ToEmail, emailParametersDto.Code, options);
        }

        public EmailValidateCodeResponseDto ValidateCode(EmailParametersDto emailParametersDto)
        {
            if (_cache.TryGetValue(emailParametersDto.ToEmail, out string? storedCode))
            {
                if (storedCode == emailParametersDto.Code)
                {
                    _cache.Remove(emailParametersDto.ToEmail);
                    return new EmailValidateCodeResponseDto { Message = "Código válido", StateCode = 200 };
                }
            }
            return new EmailValidateCodeResponseDto { Message = "Código no válido", StateCode = 400 };
        }

        /// <summary>
        /// Mantiene la compatibilidad con la interfaz. Convierte el <see cref="MimeMessage"/>
        /// en una solicitud de la API y lo envía a través de Hostinger Mail.
        /// </summary>
        public async Task<bool> SendEmailWithAlternativeConfigAsync(MimeMessage message)
        {
            try
            {
                var request = BuildRequestFromMimeMessage(message);
                await SendViaApiAsync(request);
                return true;
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Error enviando email (API): {ex.Message}", "SendEmailWithAlternativeConfigAsync");
                throw;
            }
        }

        /// <summary>
        /// Verifica la conectividad con la API de Hostinger Mail realizando un envío de prueba.
        /// </summary>
        public async Task<ResponseContract<string>> TestSmtpConnectivity()
        {
            try
            {
                var request = new HostingerSendEmailRequest
                {
                    To = new List<string> { _emailSettings.FromEmail },
                    DisplayName = _emailSettings.FromName,
                    Subject = "Test de conectividad API Hostinger Mail",
                    Text = "Este es un test de conectividad con la API de Hostinger Mail."
                };

                await SendViaApiAsync(request);
                return ResponseContract<string>.Ok(
                    "Test de conectividad con la API de Hostinger Mail exitoso",
                    "Conectividad con la API verificada correctamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<string>.Fail($"Test de conectividad con la API de Hostinger Mail falló: {ex.Message}");
            }
        }

        private HostingerSendEmailRequest BuildRequestFromMimeMessage(MimeMessage message)
        {
            var toList = message.To.Mailboxes.Select(m => m.Address).ToList();

            return new HostingerSendEmailRequest
            {
                To = toList.Count > 0 ? toList : new List<string> { _emailSettings.FromEmail },
                DisplayName = message.To.Mailboxes.FirstOrDefault()?.Name ?? _emailSettings.FromName,
                Subject = message.Subject ?? string.Empty,
                Text = message.TextBody,
                Html = message.HtmlBody
            };
        }

        private async Task LogErrorAsync(string errorMessage, string location)
        {
            try
            {
                LogError log = new()
                {
                    ModuleErrorId = WellKnownIds.GeneralModule,
                    ErrorMessage = errorMessage,
                    Location = location
                };
                await _logErrorRepository.AddLogError(log);
            }
            catch
            {
                // Si no podemos guardar el log, no queremos fallar completamente
            }
        }
    }
}


