namespace BocciaCoaching.Models.DTO.MicrocycleType
{
    public class CreateMicrocycleTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<DayPercentageDto> Days { get; set; } = new();
    }

    public class DayPercentageDto
    {
        public string DayOfWeek { get; set; } = string.Empty;
        public double ThrowPercentage { get; set; }
    }
}

