using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("TeamUser")]
    public class TeamUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdTeamUser { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public DateTime DateCreation { get; set; }
    }
}
