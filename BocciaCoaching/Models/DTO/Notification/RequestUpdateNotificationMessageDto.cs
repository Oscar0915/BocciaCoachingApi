namespace BocciaCoaching.Models.DTO.Notification
{
    public class RequestUpdateNotificationMessageDto
    {
        public int NotificationMessageId { get; set; }
        public string? Message { get; set; }
        public string? Image { get; set; }
        public int CoachId { get; set; }
        public int AthleteId { get; set; }
        public int NotificationTypeId { get; set; }
        public bool? Status { get; set; }
    }
}

