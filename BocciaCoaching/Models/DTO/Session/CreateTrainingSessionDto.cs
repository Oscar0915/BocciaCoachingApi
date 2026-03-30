namespace BocciaCoaching.Models.DTO.Session
{
    public class CreateTrainingSessionDto
    {
        public int MicrocycleId { get; set; }

        /// <summary>lunes, martes, miercoles, jueves, viernes, sabado, domingo</summary>
        public string DayOfWeek { get; set; } = string.Empty;

        /// <summary>Duración en minutos</summary>
        public int Duration { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        /// <summary>Porcentaje de lanzamientos (ej: 25 = 25%)</summary>
        public double ThrowPercentage { get; set; }

        /// <summary>Base total de lanzamientos que representa el 100% (ej: 1000)</summary>
        public int TotalThrowsBase { get; set; } = 1000;

        /// <summary>Secciones organizadas por parte. Si no se envían, se crean las 4 partes vacías.</summary>
        public List<CreateSessionPartDto>? Parts { get; set; }
    }

    public class CreateSessionPartDto
    {
        /// <summary>Propulsion, Saremas, 2x1, Escenarios de juego</summary>
        public string Name { get; set; } = string.Empty;

        public int Order { get; set; }

        public List<CreateSessionSectionDto>? Sections { get; set; }
    }

    public class CreateSessionSectionDto
    {
        public string Name { get; set; } = string.Empty;
        public int NumberOfThrows { get; set; }

        /// <summary>true = diagonal propia, false = diagonal del rival</summary>
        public bool IsOwnDiagonal { get; set; } = true;

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Observation { get; set; }
    }
}

