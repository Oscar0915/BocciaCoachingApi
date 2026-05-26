namespace BocciaCoaching.Models.DTO.Macrocycle
{
    public class MacrocycleResponseDto
    {
        public string MacrocycleId { get; set; } = string.Empty;
        public int AthleteId { get; set; }
        public string AthleteName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Notes { get; set; }
        public int CoachId { get; set; }
        public int TeamId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<MacrocycleEventResponseDto> Events { get; set; } = new();
        public List<MacrocyclePeriodResponseDto> Periods { get; set; } = new();
        public List<MesocycleResponseDto> Mesocycles { get; set; } = new();
        public List<MicrocycleResponseDto> Microcycles { get; set; } = new();
    }

    public class MacrocycleEventResponseDto
    {
        public string MacrocycleEventId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
    }

    public class MacrocyclePeriodResponseDto
    {
        public int MacrocyclePeriodId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Weeks { get; set; }
    }

    public class MesocycleResponseDto
    {
        public int MesocycleId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Weeks { get; set; }
        public string? Objective { get; set; }
    }

    public class MicrocycleResponseDto
    {
        public int MicrocycleId { get; set; }
        public int Number { get; set; }
        public int WeekNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? PeriodName { get; set; }
        public string? MesocycleName { get; set; }
        public bool HasPeakPerformance { get; set; }
        public TrainingDistributionDto? TrainingDistribution { get; set; }

        /// <summary>Id del tipo de microciclo del catálogo (nullable)</summary>
        public string? MicrocycleTypeId { get; set; }

        /// <summary>Nombre del tipo de microciclo del catálogo (nullable)</summary>
        public string? MicrocycleTypeName { get; set; }

        /// <summary>Días de la semana con sus porcentajes de lanzamiento para este microciclo</summary>
        public List<MicrocycleDayResponseDto> Days { get; set; } = new();
    }

    public class MicrocycleDayResponseDto
    {
        public string MicrocycleDayId { get; set; } = string.Empty;
        public string DayOfWeek { get; set; } = string.Empty;
        public double ThrowPercentage { get; set; }

        /// <summary>false = heredado del catálogo, true = personalizado</summary>
        public bool IsCustom { get; set; }
    }
}

