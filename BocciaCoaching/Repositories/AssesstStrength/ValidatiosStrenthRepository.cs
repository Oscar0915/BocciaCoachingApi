using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IAssesstStrength;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories;

public class ValidatiosStrenthRepository: IValidationsAssetsStrength
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public ValidatiosStrenthRepository(ApplicationDbContext context) { _context = context; }

   

    /// <summary>
    /// Validar que el equipo esta activo
    /// </summary>
    public async Task<bool> IsActiveTeam(Team team)
    {
        try
        {
            bool?  isState = await _context.Teams.Where(a =>
                    a.TeamId == team.TeamId)
                .Select(a => a.Status)
                .FirstOrDefaultAsync();
            
            return isState != null;
        }catch(Exception)
        {
            return false;
        }
    }

    public async Task<bool> IsUpdateDetailAssessStrength(
        RequestAddDetailToEvaluationForAthlete requestAddDetailToEvaluationForAthlete)
    {
        try
        {
            var idUpdate = await _context.EvaluationDetailStrengths.Where(a =>
                a.ThrowOrder == requestAddDetailToEvaluationForAthlete.ThrowOrder
                && a.AssessStrengthId == requestAddDetailToEvaluationForAthlete.AssessStrengthId).ToListAsync();
            
            if(idUpdate.Count == 0)
                return false;
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    /// <summary>
    /// Validar que el equipo esta activo
    /// </summary>
    public void ExampleMethod()
    {
        
    }
    

    
}