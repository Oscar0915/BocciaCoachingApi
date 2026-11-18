using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Services;
using BocciaCoaching.Services.Interfaces;
using BocciaCoaching.Utils;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using BocciaCoaching.Infrastructure.Auth;
using BocciaCoaching.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController: ControllerBase
    {

        private readonly IEmailService _email;
        private readonly IAuthService _auth;

        public EmailController(IEmailService user, IAuthService auth)
        {
            _email = user;
            _auth = auth;
        }


        [HttpPost("SendCodeVerify")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        public async Task<ActionResult<IEnumerable<bool>>> SendCodeVerify(EmailParametersDto emailParametersDto)
        {
            await _auth.SendVerificationCodeAsync(emailParametersDto.ToEmail);

            return Ok("Código enviado");
        }


        /// <summary>
        /// Valida el código enviado al correo (máx. 5 intentos). Si es válido devuelve <c>resetToken</c>,
        /// de un solo uso y válido 10 minutos, que exige <c>User/ResetPassword</c>.
        /// </summary>
        [HttpPost("ValidateCode")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        public async Task<ActionResult<EmailValidateCodeResponseDto>> ValidateCode(EmailParametersDto emailParametersDto)
        {
            var result = await _auth.VerifyCodeAsync(emailParametersDto.ToEmail, emailParametersDto.Code);

            return Ok(new EmailValidateCodeResponseDto
            {
                Message = result.Message,
                StateCode = result.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest,
                ResetToken = result.Data?.ResetToken
            });
        }

        /// <summary>
        /// Test de conectividad SMTP
        /// </summary>
        [HttpGet("TestSmtpConnectivity")]
        [Authorize(Roles = AuthRoles.Admin)]
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
        [Authorize(Roles = AuthRoles.Admin)]
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
