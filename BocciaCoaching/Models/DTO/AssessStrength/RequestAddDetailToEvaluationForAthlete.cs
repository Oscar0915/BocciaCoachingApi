﻿namespace BocciaCoaching.Models.DTO.AssessStrength
{
    public class RequestAddDetailToEvaluationForAthlete
    {
        public int BoxNumber { get; set; }
        public int ThrowOrder { get; set; }
        public decimal? TargetDistance { get; set; }
        public decimal? ScoreObtained { get; set; }
        public string? Observations { get; set; }
        public bool Status { get; set; } = true;
        public int AthleteId { get; set; }
        public int AssessStrengthId { get; set; }
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
        /// ES: Indica si el detalle de la evaluación corresponde a una prueba de fuerza
        /// EN: Indicates whether the evaluation detail corresponds to a strength test
        /// </summary>
        public bool IsStrength { get; set; } = false;
        /// <summary>
        /// ES: Indica si el detalle de la evaluación corresponde a una prueba de cadencia
        /// EN: Indicates whether the evaluation detail corresponds to a cadence test
        /// </summary>
        public bool IsCadence { get; set; } = false;
        /// <summary>
        /// ES: Indica si el detalle de la evaluación corresponde a una prueba de dirección
        /// EN: Indicates whether the evaluation detail corresponds to a direction test
        /// </summary>
        public bool IsDirection { get; set; } = false;
        /// <summary>
        /// ES: Indica si el detalle de la evaluación corresponde a una prueba de trayectoria
        /// EN: Indicates whether the evaluation detail corresponds to a trajectory test
        /// </summary>
        public bool IsTrajectory { get; set; } = false;
    }
}
