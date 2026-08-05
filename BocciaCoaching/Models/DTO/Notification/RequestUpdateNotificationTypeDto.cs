namespace BocciaCoaching.Models.DTO.Notification
{
    public class RequestUpdateNotificationTypeDto
    {
        public Guid NotificationTypeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
    }
}

