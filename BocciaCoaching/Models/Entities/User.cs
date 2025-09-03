using BocciaCoaching.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("User")]
    public class User: IAuditable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int UserId { get; set; }

        public string? Dni { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Address { get; set; }

        public DateTime? Seniority { get; set; }

        public bool Status { get; set; } = true;

        public Session? Session { get; set; }
        public DateTime CreatedAt { get ; set ; }
        public DateTime? UpdatedAt { get ; set ; }
    }
}
