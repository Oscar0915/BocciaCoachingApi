﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("NotificationMessage")]
    public class NotificationMessage
    {
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        [Key]
        public Guid NotificationMessageId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public bool? Status { get; set; } = true;
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? Image {  get; set; }
        /// <summary>
        /// ES: ID del usuario que envía la notificación
        /// EN: Sender user ID
        /// </summary>
        public Guid SenderId { get; set; }
        /// <summary>
        /// ES: Usuario que envía la notificación
        /// EN: Sender user
        /// </summary>
        public User Sender { get; set; }
        /// <summary>
        /// ES: ID del usuario que recibe la notificación
        /// EN: Receiver user ID
        /// </summary>
        public Guid ReceiverId { get; set; }
        /// <summary>
        /// ES: Usuario que recibe la notificación
        /// EN: Receiver user
        /// </summary>
        public User Receiver { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public Guid NotificationTypeId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public NotificationType NotificationType { get; set; }
        /// <summary>
        /// ES: ID de referencia para invitaciones o acciones relacionadas (ej: TeamId para invitación a equipo)
        /// EN: Reference ID for invitations or related actions (e.g., TeamId for team invitation)
        /// </summary>
        public Guid? ReferenceId { get; set; }
    }
}
