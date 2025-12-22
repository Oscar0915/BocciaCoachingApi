using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Services;
using BocciaCoaching.Services.Interfaces;
using BocciaCoaching.Utils;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController: ControllerBase
    {

        private readonly IEmailService _email;

        public EmailController(IEmailService user)
        {
            _email = user;
        }


        [HttpPost("SendCodeVerify")]
        public async Task<ActionResult<IEnumerable<bool>>> SendCodeVerify(EmailParametersDto emailParametersDto)
        {
            var code = SecurityCodeGenerator.GenerateCode();

            emailParametersDto.Code = code;

            _email.SaveCode(emailParametersDto);

            await _email.SendSecurityCodeAsync(emailParametersDto);

            return Ok("Código enviado");
        }


        [HttpPost("ValidateCode")]
        public async Task<ActionResult<IEnumerable<EmailValidateCodeResponseDto>>> ValidateCode(EmailParametersDto emailParametersDto)
        {
            var isValid = _email.ValidateCode(emailParametersDto);

            return Ok(isValid);
        }

        /// <summary>
        /// Test de conectividad SMTP
        /// </summary>
        [HttpGet("TestSmtpConnectivity")]
        public async Task<ActionResult<object>> TestSmtpConnectivity()
        {
            try
            {
                var result = await _email.TestSmtpConnectivity();
                
                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        message = result.Message,
                        timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message,
                        timestamp = DateTime.UtcNow
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al probar conectividad SMTP",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Envía un email de prueba
        /// </summary>
        [HttpPost("SendTestEmail")]
        public async Task<ActionResult<object>> SendTestEmail([FromQuery] string? toEmail = null)
        {
            try
            {
                var testEmailParametersDto = new EmailParametersDto
                {
                    ToEmail = toEmail ?? "test@example.com",
                    Code = SecurityCodeGenerator.GenerateCode()
                };

                await _email.SendSecurityCodeAsync(testEmailParametersDto);
                
                return Ok(new
                {
                    success = true,
                    message = "Email de prueba enviado correctamente",
                    timestamp = DateTime.UtcNow,
                    sentTo = testEmailParametersDto.ToEmail
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al enviar el email de prueba",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
