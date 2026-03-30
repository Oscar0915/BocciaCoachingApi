namespace BocciaCoaching.Models.DTO.Session
{
    /// <summary>
    /// DTO para consultar sesiones de un atleta en un rango de fechas
    /// </summary>
    public class GetAthleteSessionsDto
    {
        /// <summary>Identificador del atleta</summary>
        public int AthleteId { get; set; }

        /// <summary>Fecha de inicio del rango</summary>
        public DateTime StartDate { get; set; }

        /// <summary>Fecha de fin del rango</summary>
        public DateTime EndDate { get; set; }
    }

    /// <summary>
    /// DTO para que el atleta inicie o finalice una sesión
    /// </summary>
    public class AthleteUpdateSessionStatusDto
    {
        /// <summary>Identificador de la sesión de entrenamiento</summary>
        public int TrainingSessionId { get; set; }

        /// <summary>Identificador del atleta (para validar que la sesión le pertenece)</summary>
        public int AthleteId { get; set; }
    }

    /// <summary>
    /// Resumen de sesión con información del microciclo y macrociclo para el atleta
    /// </summary>
    public class AthleteSessionSummaryDto
    {
        public int TrainingSessionId { get; set; }
        public int MicrocycleId { get; set; }
        public string MacrocycleName { get; set; } = string.Empty;
        public int MicrocycleNumber { get; set; }
        public DateTime MicrocycleStartDate { get; set; }
        public DateTime MicrocycleEndDate { get; set; }
        public string MicrocycleType { get; set; } = string.Empty;
        public string DayOfWeek { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double ThrowPercentage { get; set; }
        public int MaxThrows { get; set; }
        public int TotalParts { get; set; }
        public int TotalSections { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

