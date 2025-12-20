using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.Email;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendSecurityCodeAsync(EmailParametersDto emailParametersDto);
        void SaveCode(EmailParametersDto emailParametersDto);
        EmailValidateCodeResponseDto ValidateCode(EmailParametersDto emailParametersDto);
        
        // Nuevos métodos para notificaciones por email
        Task<bool> SendEmailNotificationAsync(EmailNotificationDto emailNotification);
        Task<bool> SendTeamInvitationEmailAsync(TeamInvitationEmailDto invitation);
        Task<bool> SendGeneralNotificationEmailAsync(GeneralNotificationEmailDto notification);
    }
}
