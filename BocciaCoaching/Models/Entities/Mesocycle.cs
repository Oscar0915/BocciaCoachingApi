using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Mesocycle")]
    public class Mesocycle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int MesocycleId { get; set; }

        public string MacrocycleId { get; set; } = string.Empty;

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

