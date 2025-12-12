namespace BocciaCoaching.Models.DTO.Notification
{
    /// <summary>
    /// DTO para enviar invitación a un atleta para unirse a un equipo
    /// </summary>
    public class SendTeamInvitationDto
    {
        /// <summary>
        /// ID del entrenador que envía la invitación
        /// </summary>
        public int CoachId { get; set; }
        
        /// <summary>
        /// Email del atleta que recibe la invitación
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// ID del equipo al que se invita
        /// </summary>
        public int TeamId { get; set; }
        
        /// <summary>
        /// Mensaje personalizado de la invitación (opcional)
        /// </summary>
        public string? Message { get; set; }
    }
}

