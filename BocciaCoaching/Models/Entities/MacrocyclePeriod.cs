using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("MacrocyclePeriod")]
    public class MacrocyclePeriod
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int MacrocyclePeriodId { get; set; }

        public string MacrocycleId { get; set; } = string.Empty;

        [ForeignKey("MacrocycleId")]
        public Macrocycle? Macrocycle { get; set; }

        public string Name { get; set; } = string.Empty;

        /// <summary>preparatorioGeneral, preparatorioEspecial, competitivo, transicion, precompetitivo</summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>Código corto de la etapa, ej: G1, E1, P1, C1, G2, E2, P2, C2, TO</summary>
        [MaxLength(10)]
        public string? StageCode { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Weeks { get; set; }
    }
}

