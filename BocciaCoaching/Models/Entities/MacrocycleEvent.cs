using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("MacrocycleEvent")]
    public class MacrocycleEvent
    {
        [Key]
        public string MacrocycleEventId { get; set; } = Guid.NewGuid().ToString();

        public string MacrocycleId { get; set; } = string.Empty;

        [ForeignKey("MacrocycleId")]
        public Macrocycle? Macrocycle { get; set; }

        public string Name { get; set; } = string.Empty;

        /// <summary>competencia, concentracion, evaluacion, descanso, campus, controlTecnico, intercambio</summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Nivel del evento (aplicable a competencias): local, nacional, internacional.
        /// También puede usarse para clasificar evaluaciones o controles.
        /// </summary>
        [MaxLength(50)]
        public string? Level { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? Location { get; set; }
        public string? Notes { get; set; }
    }
}

