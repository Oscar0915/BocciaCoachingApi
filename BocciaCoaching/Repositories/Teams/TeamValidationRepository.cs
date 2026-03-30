using BocciaCoaching.Data;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.Teams;

public class TeamValidationRepository (ApplicationDbContext context):ITeamValidationRepository
{
    public async Task<bool> ValidateTeam(Team team)
    {
        try
        {
            var stateTeam = await context.Teams.Where(t => t.TeamId == team.TeamId)
                .Select(t => t.Status)
                .FirstOrDefaultAsync();

            // Si Status es null (equipos creados antes de la migración), se trata como activo
            return stateTeam ?? true;
        }catch (Exception)
        {
            return false;
        }
    }
}