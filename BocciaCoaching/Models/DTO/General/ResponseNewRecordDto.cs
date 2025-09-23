namespace BocciaCoaching.Models.DTO.General
{
    public class ResponseNewRecordDto
    {
        public ResponseNewRecordDto()
        {
            Success = true;
            Message = string.Empty;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
