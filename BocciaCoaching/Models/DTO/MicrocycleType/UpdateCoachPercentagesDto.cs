namespace BocciaCoaching.Models.DTO.MicrocycleType
{
    public class UpdateCoachPercentagesDto
    {
        public int CoachId { get; set; }
        public string MicrocycleTypeId { get; set; } = string.Empty;
        public List<DayPercentageDto> Days { get; set; } = new();
    }
}

