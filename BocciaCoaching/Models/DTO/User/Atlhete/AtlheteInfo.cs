namespace BocciaCoaching.Models.DTO.User.Atlhete;

public class AtlheteInfo
{
    public AtlheteInfo()
    {
        Name = string.Empty;
    }

    public int AthleteId { get; set; }
    public string Name { get; set; }
    
}