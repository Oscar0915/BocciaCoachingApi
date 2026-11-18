using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.Email;
using BocciaCoaching.Models.DTO.General;
using MimeKit;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendSecurityCodeAsync(EmailParametersDto emailParametersDto);
        void SaveCode(EmailParametersDto emailParametersDto);
        EmailValidateCodeResponseDto ValidateCode(EmailParametersDto emailParametersDto);

        /// <summary>ES: Invalida el código pendiente del email (p. ej. tras demasiados intentos fallidos).</summary>
        void InvalidateCode(string email);
        
        // Métodos para notificaciones por email
        Task<bool> SendEmailNotificationAsync(EmailNotificationDto emailNotification);
        Task<bool> SendTeamInvitationEmailAsync(TeamInvitationEmailDto invitation);
        Task<bool> SendGeneralNotificationEmailAsync(GeneralNotificationEmailDto notification);
        
        // Nuevos métodos para configuración alternativa y testing
        Task<bool> SendEmailWithAlternativeConfigAsync(MimeMessage message);
        Task<ResponseContract<string>> TestSmtpConnectivity();
    }
}
