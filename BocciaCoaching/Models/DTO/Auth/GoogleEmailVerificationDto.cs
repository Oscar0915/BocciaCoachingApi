namespace BocciaCoaching.Models.DTO.Auth;

public class GoogleEmailVerificationDto
{
    public string Email { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
}
