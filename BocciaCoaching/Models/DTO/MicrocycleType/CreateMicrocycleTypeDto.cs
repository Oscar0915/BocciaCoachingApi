namespace BocciaCoaching.Models.DTO.MicrocycleType
{
    public class CreateMicrocycleTypeDto
    {
        public string Name { get; set; } = string.Empty;

        /// <summary>Código corto (símbolo) del tipo de microciclo, ej: μ1, μ2, μ3</summary>
        public string? ShortCode { get; set; }

        public string? Description { get; set; }
        public List<DayPercentageDto> Days { get; set; } = new();
    }

    public class DayPercentageDto
    {
        public string DayOfWeek { get; set; } = string.Empty;
        public double ThrowPercentage { get; set; }
    }

    public class CreateMicrocycleTypeDayDefaultDto
    {
        /// <summary>Id del tipo de microciclo al que pertenece este día</summary>
        public string MicrocycleTypeId { get; set; } = string.Empty;

        /// <summary>Día de la semana: lunes, martes, miercoles, jueves, viernes, sabado, domingo</summary>
        public string DayOfWeek { get; set; } = string.Empty;

        /// <summary>Porcentaje de lanzamientos para ese día (ej: 25.0 = 25%)</summary>
        public double ThrowPercentage { get; set; }
    }

    public class MicrocycleTypeDayDefaultResponseDto
    {
        public string MicrocycleTypeDayDefaultId { get; set; } = string.Empty;
        public string MicrocycleTypeId { get; set; } = string.Empty;
        public string DayOfWeek { get; set; } = string.Empty;
        public double ThrowPercentage { get; set; }
    }
}

