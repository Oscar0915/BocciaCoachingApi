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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SessionId { get; set; }
        /// <summary>
        /// ES: Identificador del usuario
        /// EN: 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// ES: 
        /// EN: 
        /// </summary>
        public User User { get; set; } = null!;


    }
}
