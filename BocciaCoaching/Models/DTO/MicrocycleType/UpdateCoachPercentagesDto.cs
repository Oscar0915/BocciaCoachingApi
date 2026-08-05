namespace BocciaCoaching.Models.DTO.MicrocycleType
{
    public class UpdateCoachPercentagesDto
    {
        public Guid CoachId { get; set; }
        public Guid MicrocycleTypeId { get; set; }
        public List<DayPercentageDto> Days { get; set; } = new();
    }
}

