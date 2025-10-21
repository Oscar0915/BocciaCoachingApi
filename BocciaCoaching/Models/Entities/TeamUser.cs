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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdTeamUser { get; set; }

        /// <summary>
        /// ES: Identificador del usuario
        /// EN: 
        /// </summary>
        public int UserId { get; set; }
        public User User { get; set; }
        /// <summary>
        /// ES: Identificador del equipo
        /// EN: 
        /// </summary>
        public int TeamId { get; set; }
        public Team Team { get; set; }

        /// <summary>
        /// ES: Fecha de creación del equipo
        /// EN: 
        /// </summary>
        public DateTime DateCreation { get; set; }
    }
}
