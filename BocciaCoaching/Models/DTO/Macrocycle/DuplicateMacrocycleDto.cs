namespace BocciaCoaching.Models.DTO.Macrocycle
{
    public class DuplicateMacrocycleDto
    {
        public int NewAthleteId { get; set; }
        public string NewAthleteName { get; set; } = string.Empty;
        public DateTime? NewStartDate { get; set; }
    }
}

