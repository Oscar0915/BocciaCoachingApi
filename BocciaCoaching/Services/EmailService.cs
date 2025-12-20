using BocciaCoaching.Data;
using BocciaCoaching.Models.Configuration;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.Email;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories;
using BocciaCoaching.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Reflection;

namespace BocciaCoaching.Services
{
    public class EmailService : IEmailService
    {
        private readonly LogErrorRepository logErrorRepository;
        private readonly EmailSettings _emailSettings;
        private readonly IMemoryCache _cache;

        public EmailService(IMemoryCache cache, ApplicationDbContext context, IOptions<EmailSettings> emailSettings)
        {
            _cache = cache;
            _emailSettings = emailSettings.Value;
            logErrorRepository = new LogErrorRepository(context);
        }

        public async Task SendSecurityCodeAsync(EmailParametersDto emailParametersDto)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                message.To.Add(new MailboxAddress("", emailParametersDto.ToEmail));
                message.Subject = "Código de verificación";

                message.Body = new TextPart("plain")
                {
                    Text = $"Tu código de verificación es: {emailParametersDto.Code}\nEste código expira en 5 minutos."
                };

                await SendEmailAsync(message);

                LogError _log = new()
                {
                    ModuleErrorId = 1,
                    ErrorMessage = "Sin error",
                    Location = MethodBase.GetCurrentMethod()?.Name ?? "SendSecurityCodeAsync"
                };
                await logErrorRepository.AddLogError(_log);
            }
            catch (Exception ex)
            {
                LogError _log = new()
                {
                    ModuleErrorId = 1,
                    ErrorMessage = ex.Message,
                    Location = MethodBase.GetCurrentMethod()?.Name ?? "SendSecurityCodeAsync"
                };
                await logErrorRepository.AddLogError(_log);
                throw;
            }
        }

        public async Task<bool> SendEmailNotificationAsync(EmailNotificationDto emailNotification)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                message.To.Add(new MailboxAddress(emailNotification.ToName, emailNotification.ToEmail));
                message.Subject = emailNotification.Subject;

                if (emailNotification.IsHtml)
                {
                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = emailNotification.HtmlBody,
                        TextBody = emailNotification.PlainTextBody
                    };
                    message.Body = bodyBuilder.ToMessageBody();
                }
                else
                {
                    message.Body = new TextPart("plain")
                    {
                        Text = emailNotification.PlainTextBody
                    };
                }

                await SendEmailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Error enviando email general: {ex.Message}", "SendEmailNotificationAsync");
                return false;
            }
        }

        public async Task<bool> SendTeamInvitationEmailAsync(TeamInvitationEmailDto invitation)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                message.To.Add(new MailboxAddress(invitation.AthleteName, invitation.ToEmail));
                message.Subject = $"Invitación al equipo {invitation.TeamName}";

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

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlBody,
                    TextBody = plainTextBody
                };
                message.Body = bodyBuilder.ToMessageBody();

                await SendEmailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Error enviando invitación de equipo: {ex.Message}", "SendTeamInvitationEmailAsync");
                return false;
            }
        }

        public async Task<bool> SendGeneralNotificationEmailAsync(GeneralNotificationEmailDto notification)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                message.To.Add(new MailboxAddress(notification.RecipientName, notification.ToEmail));
                message.Subject = notification.NotificationTitle;

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

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlBody,
                    TextBody = plainTextBody
                };
                message.Body = bodyBuilder.ToMessageBody();

                await SendEmailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"Error enviando notificación general: {ex.Message}", "SendGeneralNotificationEmailAsync");
                return false;
            }
        }

        private async Task SendEmailAsync(MimeMessage message)
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private async Task LogErrorAsync(string errorMessage, string location)
        {
            try
            {
                LogError _log = new()
                {
                    ModuleErrorId = 1,
                    ErrorMessage = errorMessage,
                    Location = location
                };
                await logErrorRepository.AddLogError(_log);
            }
            catch
            {
                // Si no podemos guardar el log, no queremos fallar completamente
            }
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
    }
}
