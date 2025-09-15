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
    }
}
