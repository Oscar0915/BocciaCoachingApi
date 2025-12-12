namespace BocciaCoaching.Models.DTO.Notification
{
    public class NotificationTypeDto
    {
        public int NotificationTypeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

