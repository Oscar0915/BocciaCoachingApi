using BocciaCoaching.Data;
using BocciaCoaching.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories;

public class ValidatiosStrenthRepository
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
    
    /// <summary>
    /// Validar que el equipo esta activo
    /// </summary>
    public void ExampleMethod()
    {
        
    }
    
    
}