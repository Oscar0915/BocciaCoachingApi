using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Session")]
    public class Session
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SessionId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;


    }
}
