namespace BocciaCoaching.Models.DTO.Notification
{
    public class RequestUpdateNotificationMessageDto
    {
        public int NotificationMessageId { get; set; }
        public string? Message { get; set; }
        public string? Image { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int NotificationTypeId { get; set; }
        public bool? Status { get; set; }
    }
}

