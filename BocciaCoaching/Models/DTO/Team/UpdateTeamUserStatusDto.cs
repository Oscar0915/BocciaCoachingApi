namespace BocciaCoaching.Models.DTO.Team
{
    public class UpdateTeamUserStatusDto
    {
        /// <summary>
        /// ID del usuario en el equipo
        /// </summary>
        public int TeamUserId { get; set; }

        /// <summary>
        /// Nuevo estado del usuario en el equipo
        /// </summary>
        public bool Status { get; set; }
    }
}
