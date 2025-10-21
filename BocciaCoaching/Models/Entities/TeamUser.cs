using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("TeamUser")]
    public class TeamUser
    {
        /// <summary>
        /// Campo: Identificador de la tabla
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdTeamUser { get; set; }

        /// <summary>
        /// Campo: Identificador del usuario
        /// Field: 
        /// </summary>
        public int UserId { get; set; }
        public User User { get; set; }
        /// <summary>
        /// Campo: Identificador del equipo
        /// Field: 
        /// </summary>
        public int TeamId { get; set; }
        public Team Team { get; set; }

        /// <summary>
        /// Campo: Fecha de creación del equipo
        /// Field: 
        /// </summary>
        public DateTime DateCreation { get; set; }
    }
}
