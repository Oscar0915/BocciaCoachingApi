using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Session")]
    public class Session
    {
        /// <summary>
        /// ES:Identificador de la tabla Session
        /// EN: 
        /// </summary>
        [Key]
        public Guid SessionId { get; set; }
        /// <summary>
        /// ES: Identificador del usuario
        /// EN: 
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// ES: 
        /// EN: 
        /// </summary>
        public User User { get; set; } = null!;


    }
}
