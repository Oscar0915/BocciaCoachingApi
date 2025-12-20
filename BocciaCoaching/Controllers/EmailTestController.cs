using BocciaCoaching.Models.DTO.Email;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailTestController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailTestController(IEmailService emailService)
        {
            _emailService = emailService;
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
    }

    public class TestEmailRequestDto
    {
        public string ToEmail { get; set; } = string.Empty;
        public string? ToName { get; set; }
    }
}
