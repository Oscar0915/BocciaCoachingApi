using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("UserRol")]
    public class UserRol
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public int RolId { get; set; }
        public Rol? Rol { get; set; }
        public DateTime DateCreation { get; set; }

        public UserRol()
        {
            DateCreation = DateTime.Now;
        }
    }
}
