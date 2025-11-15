using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.ITeams;

public interface ITeamValidationRepository
{
    /// <summary>
    /// Validar estado del equipo
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    Task<bool> ValidateTeam(Team team);
}