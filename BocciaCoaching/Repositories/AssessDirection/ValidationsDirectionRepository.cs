using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.AssessDirection;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IAssessDirection;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories;

public class ValidationsDirectionRepository : IValidationsAssetsDirection
{
    private readonly ApplicationDbContext _context;

    public ValidationsDirectionRepository(ApplicationDbContext context) { _context = context; }

    public async Task<bool> IsActiveTeam(Team team)
    {
        try
        {
            bool? isState = await _context.Teams.Where(a =>
                    a.TeamId == team.TeamId)
                .Select(a => a.Status)
                .FirstOrDefaultAsync();

            return isState != null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> IsUpdateDetailAssessDirection(
        RequestAddDetailToDirectionEvaluation requestAddDetailToDirectionEvaluation)
    {
        try
        {
            var idUpdate = await _context.EvaluationDetailDirections.Where(a =>
                a.ThrowOrder == requestAddDetailToDirectionEvaluation.ThrowOrder
                && a.AssessDirectionId == requestAddDetailToDirectionEvaluation.AssessDirectionId
                && a.AthleteId == requestAddDetailToDirectionEvaluation.AthleteId).ToListAsync();

            if (idUpdate.Count == 0)
                return false;
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

