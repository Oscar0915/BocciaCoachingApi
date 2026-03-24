using BocciaCoaching.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("EvaluationDetailDirection")]
    public class EvaluationDetailDirection : IAuditable
    {
        /// <summary>
        /// ES: Identificador del detalle de evaluación de dirección
        /// EN: Direction evaluation detail identifier
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int EvaluationDetailDirectionId { get; set; }

        /// <summary>
        /// ES: Numero de cajón (1=Derecho, 2=Izquierdo)
        /// EN: Box number (1=Right, 2=Left)
        /// </summary>
        public int BoxNumber { get; set; }

        /// <summary>
        /// ES: Orden de lanzamiento
        /// EN: Throw order
        /// </summary>
        public int ThrowOrder { get; set; }

        /// <summary>
        /// ES: Distancia objetivo (3, 6 o 9 metros)
        /// EN: Target distance (3, 6 or 9 meters)
        /// </summary>
        public decimal? TargetDistance { get; set; }

        /// <summary>
        /// ES: Puntaje obtenido
        /// EN: Score obtained
        /// </summary>
        public decimal? ScoreObtained { get; set; }

        /// <summary>
        /// ES: Observación
        /// EN: Observation
        /// </summary>
        public string? Observations { get; set; }

        /// <summary>
        /// ES: Estado
        /// EN: Status
        /// </summary>
        public bool Status { get; set; } = true;

        /// <summary>
        /// ES: Identificación del atleta
        /// EN: Athlete identification
        /// </summary>
        public int AthleteId { get; set; }

        /// <summary>
        /// ES: Navegación al atleta
        /// EN: Navigation to athlete
        /// </summary>
        public User Athlete { get; set; }

        /// <summary>
        /// ES: Número de la evaluación de dirección
        /// EN: Direction assessment number
        /// </summary>
        public int AssessDirectionId { get; set; }

        /// <summary>
        /// ES: Navegación a la evaluación de dirección
        /// EN: Navigation to direction assessment
        /// </summary>
        public AssessDirection AssessDirection { get; set; }

        /// <summary>
        /// ES: Fecha de creación
        /// EN: Created at
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// ES: Fecha de actualización
        /// EN: Updated at
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

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

        /// <summary>
        /// ES: Indica si el lanzamiento se desvió a la derecha
        /// EN: Indicates whether the throw deviated to the right
        /// </summary>
        public bool DeviatedRight { get; set; } = false;

        /// <summary>
        /// ES: Indica si el lanzamiento se desvió a la izquierda
        /// EN: Indicates whether the throw deviated to the left
        /// </summary>
        public bool DeviatedLeft { get; set; } = false;
    }
}

