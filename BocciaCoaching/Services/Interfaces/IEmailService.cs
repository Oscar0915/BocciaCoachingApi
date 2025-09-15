using BocciaCoaching.Models.DTO.Auth;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendSecurityCodeAsync(EmailParametersDto emailParametersDto);
        void SaveCode(EmailParametersDto emailParametersDto);
        EmailValidateCodeResponseDto ValidateCode(EmailParametersDto emailParametersDto);
    }
}
