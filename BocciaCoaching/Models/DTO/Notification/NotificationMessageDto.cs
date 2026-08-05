namespace BocciaCoaching.Models.DTO.Notification
{
    public class NotificationMessageDto
    {
        public Guid NotificationMessageId { get; set; }
        public string? Message { get; set; }
        public string? Image { get; set; }
        public Guid SenderId { get; set; }
        public string? SenderName { get; set; }
        public Guid ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public Guid NotificationTypeId { get; set; }
        public string? NotificationTypeName { get; set; }
        public bool? Status { get; set; }
        /// <summary>
        /// ID de referencia para invitaciones o acciones (ej: TeamId para invitación a equipo)
        /// </summary>
        public Guid? ReferenceId { get; set; }
    }
}

