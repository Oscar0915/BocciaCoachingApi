using BocciaCoaching.Data;
using BocciaCoaching.Models.Configuration;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.Email;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories;
using BocciaCoaching.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Reflection;
using System.Net.Sockets;
using System.Security.Authentication;

namespace BocciaCoaching.Services
{
    public class EmailService : IEmailService
    {
        private readonly LogErrorRepository _logErrorRepository;
        private readonly EmailSettings _emailSettings;
        private readonly IMemoryCache _cache;

        public EmailService(IMemoryCache cache, ApplicationDbContext context, IOptions<EmailSettings> emailSettings)
        {
            _cache = cache;
            _emailSettings = emailSettings.Value;
            _logErrorRepository = new LogErrorRepository(context);
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

                LogError log = new()
                {
                    ModuleErrorId = 1,
                    ErrorMessage = "Sin error",
                    Location = MethodBase.GetCurrentMethod()?.Name ?? "SendSecurityCodeAsync"
                };
                await _logErrorRepository.AddLogError(log);
            }
            catch (Exception ex)
            {
                LogError log = new()
                {
                    ModuleErrorId = 1,
                    ErrorMessage = ex.Message,
                    Location = MethodBase.GetCurrentMethod()?.Name ?? "SendSecurityCodeAsync"
                };
                await _logErrorRepository.AddLogError(log);
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
            
            try
            {
                // Configurar timeout más largo para conexiones SSL/TLS
                client.Timeout = 30000; // 30 segundos para permitir handshake SSL
                
                Console.WriteLine($"🔗 Conectando a SMTP: {_emailSettings.SmtpServer}:{_emailSettings.Port}");
                Console.WriteLine($"🔒 Configuración SSL: {_emailSettings.UseSsl}");
                
                // Configurar el tipo de conexión según el puerto y configuración SSL
                MailKit.Security.SecureSocketOptions socketOptions;
                
                if (_emailSettings.UseSsl)
                {
                    if (_emailSettings.Port == 465)
                    {
                        Console.WriteLine("🔐 Usando SSL implícito (puerto 465)");
                        socketOptions = MailKit.Security.SecureSocketOptions.SslOnConnect;
                    }
                    else if (_emailSettings.Port == 587)
                    {
                        Console.WriteLine("🔄 Usando STARTTLS (puerto 587)");
                        socketOptions = MailKit.Security.SecureSocketOptions.StartTls;
                    }
                    else
                    {
                        Console.WriteLine("🔄 Usando STARTTLS para puerto personalizado");
                        socketOptions = MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable;
                    }
                }
                else
                {
                    Console.WriteLine("⚠️ Usando conexión sin cifrado (no recomendado)");
                    socketOptions = MailKit.Security.SecureSocketOptions.None;
                }

                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, socketOptions);
                Console.WriteLine("✅ Conexión SMTP establecida");
                
                Console.WriteLine($"🔑 Autenticando como: {_emailSettings.FromEmail}");
                await client.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);
                Console.WriteLine("✅ Autenticación exitosa");
                
                await client.SendAsync(message);
                Console.WriteLine("📧 Email enviado exitosamente");
                
                await client.DisconnectAsync(true);
                Console.WriteLine("🔌 Desconexión exitosa");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"❌ Error de conexión de red: {ex.Message}");
                Console.WriteLine($"💡 Puerto {_emailSettings.Port} puede estar bloqueado por firewall");
                Console.WriteLine($"💡 Para Hostinger, pruebe:");
                Console.WriteLine($"   - Puerto 587 con STARTTLS");
                Console.WriteLine($"   - Puerto 465 con SSL");
                throw new Exception($"No se puede conectar al servidor SMTP en puerto {_emailSettings.Port}: {ex.Message}");
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine($"❌ Error de autenticación: {ex.Message}");
                Console.WriteLine($"💡 Verifique las credenciales:");
                Console.WriteLine($"   - Email: {_emailSettings.FromEmail}");
                Console.WriteLine($"   - Contraseña: [OCULTA]");
                throw new Exception($"Credenciales SMTP inválidas: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"❌ Timeout de conexión: {ex.Message}");
                Console.WriteLine($"💡 El puerto {_emailSettings.Port} puede estar bloqueado o el servidor está sobrecargado.");
                Console.WriteLine($"💡 Recomendaciones:");
                Console.WriteLine($"   - Intente puerto 587 si está usando 465");
                Console.WriteLine($"   - Intente puerto 465 si está usando 587");
                Console.WriteLine($"   - Verifique configuración de firewall");
                throw new Exception($"Timeout conectando al servidor SMTP en puerto {_emailSettings.Port}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error general en SendEmailAsync: {ex.Message}");
                Console.WriteLine($"📍 StackTrace: {ex.StackTrace}");
                
                // Información adicional para debugging
                Console.WriteLine($"📊 Información de conexión:");
                Console.WriteLine($"   - Servidor: {_emailSettings.SmtpServer}");
                Console.WriteLine($"   - Puerto: {_emailSettings.Port}");
                Console.WriteLine($"   - SSL: {_emailSettings.UseSsl}");
                Console.WriteLine($"   - Email: {_emailSettings.FromEmail}");
                
                throw;
            }
        }

        private async Task LogErrorAsync(string errorMessage, string location)
        {
            try
            {
                LogError log = new()
                {
                    ModuleErrorId = 1,
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
        /// Intenta enviar email con configuración alternativa en caso de fallo
        /// </summary>
        public async Task<bool> SendEmailWithAlternativeConfigAsync(MimeMessage message)
        {
            // Configuraciones alternativas para Hostinger
            var configs = new[]
            {
                new { Port = 587, Ssl = true, Description = "STARTTLS (587)" },
                new { Port = 465, Ssl = true, Description = "SSL (465)" },
                new { Port = 25, Ssl = false, Description = "Sin cifrado (25)" }
            };

            Exception? lastException = null;

            foreach (var config in configs)
            {
                try
                {
                    Console.WriteLine($"🔄 Intentando con {config.Description}...");
                    
                    using var client = new SmtpClient();
                    client.Timeout = 30000; // 30 segundos
                    
                    MailKit.Security.SecureSocketOptions socketOptions;
                    
                    if (config.Ssl)
                    {
                        socketOptions = config.Port == 465 
                            ? MailKit.Security.SecureSocketOptions.SslOnConnect 
                            : MailKit.Security.SecureSocketOptions.StartTls;
                    }
                    else
                    {
                        socketOptions = MailKit.Security.SecureSocketOptions.None;
                    }

                    await client.ConnectAsync(_emailSettings.SmtpServer, config.Port, socketOptions);
                    Console.WriteLine($"✅ Conexión establecida con {config.Description}");
                    
                    await client.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);
                    Console.WriteLine($"✅ Autenticación exitosa con {config.Description}");
                    
                    await client.SendAsync(message);
                    Console.WriteLine($"📧 Email enviado exitosamente con {config.Description}");
                    
                    await client.DisconnectAsync(true);
                    
                    // Si llegamos aquí, la configuración funcionó
                    Console.WriteLine($"💡 Configuración exitosa: Puerto {config.Port}, SSL: {config.Ssl}");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Falló {config.Description}: {ex.Message}");
                    lastException = ex;
                }
            }

            // Si ninguna configuración funcionó
            Console.WriteLine($"❌ Todas las configuraciones fallaron. Último error: {lastException?.Message}");
            throw lastException ?? new Exception("No se pudo enviar el email con ninguna configuración");
        }

        /// <summary>
        /// Test de conectividad SMTP
        /// </summary>
        public async Task<ResponseContract<string>> TestSmtpConnectivity()
        {
            try
            {
                var testMessage = new MimeMessage();
                testMessage.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                testMessage.To.Add(new MailboxAddress("Test", _emailSettings.FromEmail));
                testMessage.Subject = "Test de conectividad SMTP";
                testMessage.Body = new TextPart("plain") { Text = "Este es un test de conectividad SMTP." };

                await SendEmailWithAlternativeConfigAsync(testMessage);
                return ResponseContract<string>.Ok("Test de conectividad SMTP exitoso", "Conectividad SMTP verificada correctamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<string>.Fail($"Test de conectividad SMTP falló: {ex.Message}");
            }
        }
    }
}
