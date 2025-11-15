namespace BocciaCoaching.Repositories.Interfaces.IAssesstStrength;

public interface IValidationsAssetsStrength
{
    Task<bool>  IsActiveTeam(int teamId);
    Task<bool>  IsActivePlayer(int playerId);
    
}