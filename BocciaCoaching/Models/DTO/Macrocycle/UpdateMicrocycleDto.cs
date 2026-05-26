namespace BocciaCoaching.Models.DTO.Macrocycle
{
    public class UpdateMicrocycleDto
    {
        public int MicrocycleId { get; set; }
        public string MacrocycleId { get; set; } = string.Empty;
        public string? Type { get; set; }
        public bool? HasPeakPerformance { get; set; }
        public TrainingDistributionDto? TrainingDistribution { get; set; }

        /// <summary>Id del tipo de microciclo del catálogo para actualizar la relación y sus días (opcional)</summary>
        public string? MicrocycleTypeId { get; set; }

        /// <summary>Porcentajes personalizados por día. Si se envía, reemplaza los días actuales del microciclo.</summary>
        public List<MicrocycleDayUpdateDto>? Days { get; set; }
    }

    public class MicrocycleDayUpdateDto
    {
        /// <summary>Día de la semana: lunes, martes, miercoles, jueves, viernes, sabado, domingo</summary>
        public string DayOfWeek { get; set; } = string.Empty;

        /// <summary>Porcentaje de lanzamientos para ese día (ej: 25.0 = 25%)</summary>
        public double ThrowPercentage { get; set; }
    }
}

