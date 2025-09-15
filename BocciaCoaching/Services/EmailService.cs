using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Caching.Memory;
using MimeKit;

namespace BocciaCoaching.Services
{
    public class EmailService: IEmailService
    {
        private readonly string _smtpServer = "smtp.hostinger.com"; 
        private readonly int _port = 587;
        private readonly string _fromEmail = "notify@bocciacoaching.com";
        private readonly string _password = "Sr[c26g3";

        private readonly IMemoryCache _cache;

        public EmailService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task SendSecurityCodeAsync(EmailParametersDto emailParametersDto)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Boccia Coaching", _fromEmail));
            message.To.Add(new MailboxAddress("", emailParametersDto.ToEmail));
            message.Subject = "Código de verificación";

            message.Body = new TextPart("plain")
            {
                Text = $"Tu código de verificación es: {emailParametersDto.Code}\nEste código expira en 5 minutos."
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_fromEmail, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
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
                    return new EmailValidateCodeResponseDto { Message = "Código válido" , StateCode=200};
                }
            }
            return new EmailValidateCodeResponseDto { Message = "Código no válido", StateCode = 400 };
        }
    }
}
