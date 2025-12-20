namespace BocciaCoaching.Models.DTO.Email
{
    public class EmailNotificationDto
    {
        public string ToEmail { get; set; } = string.Empty;
        public string ToName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string HtmlBody { get; set; } = string.Empty;
        public string PlainTextBody { get; set; } = string.Empty;
        public bool IsHtml { get; set; } = true;
    }

    public class TeamInvitationEmailDto
    {
        public string ToEmail { get; set; } = string.Empty;
        public string AthleteName { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public string CoachName { get; set; } = string.Empty;
        public string InvitationLink { get; set; } = string.Empty;
    }

    public class GeneralNotificationEmailDto
    {
        public string ToEmail { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
        public string NotificationTitle { get; set; } = string.Empty;
        public string NotificationMessage { get; set; } = string.Empty;
        public string NotificationType { get; set; } = string.Empty;
    }
}
