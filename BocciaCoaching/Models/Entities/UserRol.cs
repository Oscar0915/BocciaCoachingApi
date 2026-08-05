using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("UserRol")]
    public class UserRol
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid RolId { get; set; }
        public Rol? Rol { get; set; }
        public DateTime DateCreation { get; set; }

        public UserRol()
        {
            DateCreation = DateTime.Now;
        }
    }
}
