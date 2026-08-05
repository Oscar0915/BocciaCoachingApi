using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Macrocycle")]
    public class Macrocycle
    {
        [Key]
        public Guid MacrocycleId { get; set; } = Guid.NewGuid();

        public Guid AthleteId { get; set; }

        [ForeignKey("AthleteId")]
        public User? Athlete { get; set; }

        public string AthleteName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? Notes { get; set; }

        public Guid CoachId { get; set; }

        [ForeignKey("CoachId")]
        public User? Coach { get; set; }

        public Guid TeamId { get; set; }

        [ForeignKey("TeamId")]
        public Team? Team { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<MacrocycleEvent> Events { get; set; } = new List<MacrocycleEvent>();
        public ICollection<MacrocyclePeriod> Periods { get; set; } = new List<MacrocyclePeriod>();
        public ICollection<Mesocycle> Mesocycles { get; set; } = new List<Mesocycle>();
        public ICollection<Microcycle> Microcycles { get; set; } = new List<Microcycle>();
    }
}

