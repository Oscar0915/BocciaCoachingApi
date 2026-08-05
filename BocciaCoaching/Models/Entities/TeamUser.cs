using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("TeamUser")]
    public class TeamUser
    {
        /// <summary>
        /// ES: Identificador de la tabla
        /// EN: 
        /// </summary>
        [Key]
        public Guid IdTeamUser { get; set; }

        /// <summary>
        /// ES: Identificador del usuario
        /// EN: 
        /// </summary>
        public Guid UserId { get; set; }
        public User? User { get; set; }
        /// <summary>
        /// ES: Identificador del equipo
        /// EN: 
        /// </summary>
        public Guid TeamId { get; set; }
        public Team? Team { get; set; }

        /// <summary>
        /// ES: Fecha de creación del equipo
        /// EN: 
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// ES: Estado del usuario en el equipo (activo/inactivo)
        /// EN: User status in the team (active/inactive)
        /// </summary>
        public bool Status { get; set; } = true;

        public TeamUser()
        {
            DateCreation = DateTime.Now;
            Status = true;
        }
    }
}
