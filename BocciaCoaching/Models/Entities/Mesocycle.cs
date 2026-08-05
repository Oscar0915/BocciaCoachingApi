using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Mesocycle")]
    public class Mesocycle
    {
        [Key]
        public Guid MesocycleId { get; set; }

        public Guid MacrocycleId { get; set; }

        [ForeignKey("MacrocycleId")]
        public Macrocycle? Macrocycle { get; set; }

        public int Number { get; set; }

        public string Name { get; set; } = string.Empty;

        /// <summary>introductorio, desarrollador, estabilizador, competitivo, recuperacion, precompetitivo</summary>
        public string Type { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Weeks { get; set; }

        public string? Objective { get; set; }
    }
}

