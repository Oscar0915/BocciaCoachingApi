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
    public class Team() : IAuditable
    {
        /// <summary>
        /// ES:Identificador del equipo
        /// EN: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TeamId { get; set; }
        /// <summary>
        /// ES: Nombre del equipo
        /// EN: 
        /// </summary>
        public string? NameTeam {  get; set; }
        /// <summary>
        /// ES: Descripción del equipo
        /// EN: 
        /// </summary>
        public string? Description {  get; set; }
        /// <summary>
        /// ES: Identificador del entrenador
        /// EN: 
        /// </summary>
        public int CoachId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public User? Coach { get; set; }
        /// <summary>
        /// ES: Estado del equipo
        /// EN: 
        /// </summary>
        public bool? Status { get; set; } = true;
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public string? Image {  get; set; }
        /// <summary>
        /// ES: Categoría BC1 hace parte del equipo
        /// EN: 
        /// </summary>
        public bool? Bc1 { get; set; }
        /// <summary>
        /// ES: Categoría BC2 hace parte del equipo
        /// EN: 
        /// </summary>
        public bool? Bc2 { get; set; }
        /// <summary>
        /// ES: Categoría BC3 hace parte del equipo
        /// EN: 
        /// </summary>
        public bool? Bc3 { get; set; }
        /// <summary>
        /// ES: Categoría BC4 hace parte del equipo
        /// EN: 
        /// </summary>
        public bool? Bc4 { get; set; }
        /// <summary>
        /// ES: Categoría parejas hace parte del equipo
        /// EN: 
        /// </summary>
        public bool? Pairs { get; set; }
        /// <summary>
        /// ES: Categoría equipos hace parte del equipo
        /// EN: 
        /// </summary>
        public bool? Teams { get; set; }
        /// <summary>
        /// ES:  Categoría equipos BC1 y BC2 hace parte del equipo
        /// EN: 
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// ES: País
        /// EN: 
        /// </summary>
        public string? Region { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public DateTime? UpdatedAt { get; set ; }
    }
}
