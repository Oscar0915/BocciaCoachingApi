namespace BocciaCoaching.Models.DTO.AssessDirection
{
    public class RequestAddDetailToDirectionEvaluation
    {
        public int BoxNumber { get; set; }
        public int ThrowOrder { get; set; }
        public decimal? TargetDistance { get; set; }
        public decimal? ScoreObtained { get; set; }
        public string? Observations { get; set; }
        public bool Status { get; set; } = true;
        public int AthleteId { get; set; }
        public int AssessDirectionId { get; set; }

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

