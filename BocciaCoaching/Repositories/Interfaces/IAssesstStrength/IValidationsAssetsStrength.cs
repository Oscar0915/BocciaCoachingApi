using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.IAssesstStrength;

public interface IValidationsAssetsStrength
{
    Task<bool>  IsActiveTeam(int teamId);
    Task<bool>  IsActivePlayer(int playerId);

    /// <summary>
    /// Número de lanzamientos por prueba realizados
    /// </summary>
    /// <returns></returns>
    Task<int> NumberOfThrowsPerTrial(AssessStrength assessStrength);

}