using BocciaCoaching.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Team")]
    public class Team : IAuditable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TeamId { get; set; }
        public string? NameTeam {  get; set; }
        public string? Description {  get; set; }
        public int CoachId { get; set; }
        public User Coach { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set ; }
    }
}
