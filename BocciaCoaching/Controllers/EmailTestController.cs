using BocciaCoaching.Models.DTO.Email;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BocciaCoaching.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using System.Net.NetworkInformation;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = AuthRoles.Admin)] // diagnóstico de correo: solo administradores
    public class EmailTestController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public EmailTestController(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        /// <summary>
        /// Endpoint de prueba para verificar el envío de emails
        /// </summary>
        [HttpPost("test-email")]
        public async Task<IActionResult> TestEmail([FromBody] TestEmailRequestDto request)
        {
            try
            {
                var emailDto = new EmailNotificationDto
                {
                    ToEmail = request.ToEmail,
                    ToName = request.ToName ?? "Usuario de Prueba",
                    Subject = "Email de Prueba - Boccia Coaching",
                    HtmlBody = $@"
                        <html>
                        <body>
                            <h2>🏆 Email de Prueba - Boccia Coaching</h2>
                            <p>Hola <strong>{request.ToName ?? "Usuario"}</strong>,</p>
                            <p>Este es un email de prueba para verificar la configuración SMTP.</p>
                            <div style='background-color: #f0f8ff; padding: 15px; border-left: 4px solid #4CAF50; margin: 20px 0;'>
                                <p><strong>✅ Configuración SMTP funcionando correctamente</strong></p>
                                <p>📧 Servidor: smtp.hostinger.com</p>
                                <p>🔒 Conexión segura establecida</p>
                            </div>
                            <p>Si recibes este email, significa que el sistema está funcionando perfectamente.</p>
                            <br>
                            <p><em>Equipo Boccia Coaching</em></p>
                        </body>
                        </html>",
                    PlainTextBody = $@"Email de Prueba - Boccia Coaching

Hola {request.ToName ?? "Usuario"},

Este es un email de prueba para verificar la configuración SMTP.

✅ Configuración SMTP funcionando correctamente
📧 Servidor: smtp.hostinger.com
🔒 Conexión segura establecida

Si recibes este email, significa que el sistema está funcionando perfectamente.

Equipo Boccia Coaching",
                    IsHtml = true
                };

                var result = await _emailService.SendEmailNotificationAsync(emailDto);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        message = $"Email de prueba enviado exitosamente a {request.ToEmail}",
                        timestamp = DateTime.UtcNow
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = "Error al enviar el email de prueba",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Diagnóstica la conectividad SMTP probando diferentes configuraciones
        /// </summary>
        [HttpPost("diagnose")]
        public async Task<IActionResult> DiagnoseSmtpConnectivity()
        {
            var results = new List<object>();
            var smtpServer = "smtp.hostinger.com";
            var email = _configuration["EmailSettings:FromEmail"];
            var password = _configuration["EmailSettings:Password"];

            // Configuraciones a probar
            var configs = new[]
            {
                new { Port = 587, Ssl = SecureSocketOptions.StartTls, Name = "STARTTLS (587)" },
                new { Port = 465, Ssl = SecureSocketOptions.SslOnConnect, Name = "SSL Implícito (465)" },
                new { Port = 25, Ssl = SecureSocketOptions.None, Name = "Sin cifrado (25)" },
                new { Port = 2525, Ssl = SecureSocketOptions.StartTls, Name = "STARTTLS Alternativo (2525)" }
            };

            foreach (var config in configs)
            {
                try
                {
                    Console.WriteLine($"🧪 Probando {config.Name}...");
                    
                    using var client = new SmtpClient();
                    client.Timeout = 10000; // 10 segundos timeout más corto
                    
                    var startTime = DateTime.UtcNow;
                    await client.ConnectAsync(smtpServer, config.Port, config.Ssl);
                    var connectionTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
                    
                    Console.WriteLine($"✅ Conexión exitosa en {connectionTime}ms");

                    // Probar autenticación solo si la conexión fue exitosa
                    try
                    {
                        await client.AuthenticateAsync(email, password);
                        await client.DisconnectAsync(true);
                        
                        results.Add(new
                        {
                            configuration = config.Name,
                            port = config.Port,
                            success = true,
                            connectionTimeMs = connectionTime,
                            authenticated = true,
                            error = (string?)null
                        });
                    }
                    catch (Exception authEx)
                    {
                        await client.DisconnectAsync(true);
                        results.Add(new
                        {
                            configuration = config.Name,
                            port = config.Port,
                            success = true,
                            connectionTimeMs = connectionTime,
                            authenticated = false,
                            error = $"Auth failed: {authEx.Message}"
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error en {config.Name}: {ex.Message}");
                    results.Add(new
                    {
                        configuration = config.Name,
                        port = config.Port,
                        success = false,
                        connectionTimeMs = 0,
                        authenticated = false,
                        error = ex.Message
                    });
                }
            }

            return Ok(new
            {
                message = "Diagnóstico SMTP completado",
                server = smtpServer,
                results = results,
                recommendation = GetRecommendation(results),
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Verifica conectividad básica de red al servidor SMTP
        /// </summary>
        [HttpPost("ping")]
        public async Task<IActionResult> PingSmtpServer()
        {
            try
            {
                var ping = new Ping();
                var reply = await ping.SendPingAsync("smtp.hostinger.com", 5000);
                
                return Ok(new
                {
                    success = reply.Status == IPStatus.Success,
                    status = reply.Status.ToString(),
                    roundtripTime = reply.RoundtripTime,
                    message = reply.Status == IPStatus.Success 
                        ? "Servidor accesible" 
                        : "Servidor no accesible",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        private string GetRecommendation(List<object> results)
        {
            // Análisis simple de resultados para dar recomendación
            var successResults = results.Where(r => 
                r.GetType().GetProperty("success")?.GetValue(r)?.ToString() == "True").ToList();
                
            if (successResults.Any())
            {
                return "Use la configuración que mostró éxito en las pruebas.";
            }
            
            return "Ninguna configuración funcionó. Verifique las credenciales o contacte con Hostinger para verificar restricciones de firewall.";
        }
    }

    public class TestEmailRequestDto
    {
        public string ToEmail { get; set; } = string.Empty;
        public string? ToName { get; set; }
    }
}
