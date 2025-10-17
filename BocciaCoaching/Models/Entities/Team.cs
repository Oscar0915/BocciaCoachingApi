using BocciaCoaching.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    /// <summary>
    /// Tabla: Equipos
    /// Table: Teams
    /// </summary>
    [Table("Team")]
    public class Team : IAuditable
    {
        /// <summary>
        /// Campo:Identificador del equipo
        /// Field: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TeamId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string? NameTeam {  get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string? Description {  get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public int CoachId { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public User? Coach { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public bool? Status { get; set; } = true;
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string? Image {  get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public bool? Bc1 { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public bool? Bc2 { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public bool? Bc3 { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public bool? Bc4 { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public bool? Pairs { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public bool? Teams { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public string? Region { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Campo:
        /// Field: 
        /// </summary>
        public DateTime? UpdatedAt { get; set ; }
    }
}
