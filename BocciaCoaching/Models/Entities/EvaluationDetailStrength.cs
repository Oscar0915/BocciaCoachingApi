using BocciaCoaching.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{

    [Table("EvaluationDetailStrength")]
    public class EvaluationDetailStrength: IAuditable
    {
        /// <summary>
        /// ES: Identificador de la evaluación
        /// EN: 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int EvaluationDetailStrengthId { get; set; }
        /// <summary>
        /// ES: Numero de cajon
        /// EN: 
        /// </summary>
        public int BoxNumber { get; set; }
        /// <summary>
        /// ES: Orden de lanzamiento
        /// EN: 
        /// </summary>
        public int ThrowOrder { get; set; }
        /// <summary>
        /// ES:Distancia objetivo
        /// EN: 
        /// </summary>
        public decimal? TargetDistance { get; set; }
        /// <summary>
        /// ES:Puntaje obtenido
        /// EN: 
        /// </summary>
        public decimal? ScoreObtained { get; set; }
        /// <summary>
        /// ES:Observación
        /// EN: 
        /// </summary>
        public string? Observations { get; set; }
        /// <summary>
        /// ES:Estado
        /// EN: 
        /// </summary>
        public bool Status { get; set; } = true;
        /// <summary>
        /// ES: Identificación del atleta
        /// EN: 
        /// </summary>
        public int AthleteId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public User Athlete {  get; set; }
        /// <summary>
        /// ES:Número de la evaluacion
        /// EN: 
        /// </summary>
        public int AssessStrengthId { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public AssessStrength AssessStrength { get; set; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public DateTime CreatedAt { get ; set ; }
        /// <summary>
        /// ES:
        /// EN: 
        /// </summary>
        public DateTime? UpdatedAt { get ; set ; }
        
        /// <summary>
        /// ES: Coordenada X del lanzamiento
        /// EN: X coordinate of the throw
        /// </summary>
        public double CoordinateX { get; set; }
        
        /// <summary>
        /// ES: Coordenada Y del lanzamiento
        /// EN: Y coordinate of the throw
        /// </summary>
        public double CoordinateY { get; set; }
    }
}
