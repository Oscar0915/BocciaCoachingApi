namespace BocciaCoaching.Models.DTO.Auth
{
    public class EmailValidateCodeResponseDto
    {
        public EmailValidateCodeResponseDto() { 
        Message = string.Empty;
            StateCode = 0;
        }
        public string Message { get; set; }
        public int StateCode { get; set; }

        /// <summary>
        /// ES: Si el código es válido, token de un solo uso (10 min) que exige <c>ResetPassword</c>.
        /// </summary>
        public string? ResetToken { get; set; }
    }
}
