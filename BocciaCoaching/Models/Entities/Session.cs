using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Session")]
    public class Session
    {
        /// <summary>
        /// Campo:Identificador de la tabla Session
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SessionId { get; set; }
        /// <summary>
        /// Campo: Identificador del usuario
        /// Field: 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Campo: 
        /// Field: 
        /// </summary>

        public User User { get; set; } = null!;


    }
}
