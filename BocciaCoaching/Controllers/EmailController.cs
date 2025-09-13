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
        public async Task<ActionResult<IEnumerable<bool>>> ValidateCode(EmailParametersDto emailParametersDto)
        {
            var isValid = _email.ValidateCode(emailParametersDto);

            return isValid
                ? Ok("Código válido ✅")
                : BadRequest("Código inválido ❌ o expirado ⏳");
        }
    }
}
