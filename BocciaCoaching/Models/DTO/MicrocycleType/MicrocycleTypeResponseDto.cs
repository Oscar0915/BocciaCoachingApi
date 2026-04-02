namespace BocciaCoaching.Models.DTO.MicrocycleType
{
    public class MicrocycleTypeResponseDto
    {
        public string MicrocycleTypeId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public List<MicrocycleTypeDayDto> Days { get; set; } = new();
    }

    public class MicrocycleTypeDayDto
    {
        public string DayOfWeek { get; set; } = string.Empty;
        public double ThrowPercentage { get; set; }

        /// <summary>true si es un porcentaje personalizado del coach, false si es el valor por defecto</summary>
        public bool IsCustom { get; set; }
    }
}

