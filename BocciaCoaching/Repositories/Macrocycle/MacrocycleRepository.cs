using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.Macrocycle;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IMacrocycle;
using Microsoft.EntityFrameworkCore;
using MacrocycleEntity = BocciaCoaching.Models.Entities.Macrocycle;

namespace BocciaCoaching.Repositories.Macrocycle
{
    public class MacrocycleRepository : IMacrocycleRepository
    {
        private readonly ApplicationDbContext _context;

        public MacrocycleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MacrocycleEntity> CreateAsync(MacrocycleEntity macrocycle)
        {
            await _context.Macrocycles.AddAsync(macrocycle);
            await _context.SaveChangesAsync();
            return macrocycle;
        }

        public async Task<List<MacrocycleSummaryDto>> GetByAthleteAsync(int athleteId)
        {
            return await _context.Macrocycles
                .Include(m => m.Coach)
                .Include(m => m.Team)
                .Where(m => m.AthleteId == athleteId)
                .OrderByDescending(m => m.StartDate)
                .Select(m => new MacrocycleSummaryDto
                {
                    MacrocycleId = m.MacrocycleId,
                    Name = m.Name,
                    AthleteName = m.AthleteName,
                    AthleteId = m.AthleteId,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    EventCount = m.Events.Count,
                    CoachId = m.CoachId,
                    CoachName = m.Coach != null ? m.Coach.FirstName + " " + m.Coach.LastName : "",
                    TeamId = m.TeamId,
                    TeamName = m.Team != null ? m.Team.NameTeam : "",
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<List<MacrocycleSummaryDto>> GetByTeamAsync(int teamId)
        {
            return await _context.Macrocycles
                .Include(m => m.Coach)
                .Include(m => m.Team)
                .Where(m => m.TeamId == teamId)
                .OrderByDescending(m => m.StartDate)
                .Select(m => new MacrocycleSummaryDto
                {
                    MacrocycleId = m.MacrocycleId,
                    Name = m.Name,
                    AthleteName = m.AthleteName,
                    AthleteId = m.AthleteId,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    EventCount = m.Events.Count,
                    CoachId = m.CoachId,
                    CoachName = m.Coach != null ? m.Coach.FirstName + " " + m.Coach.LastName : "",
                    TeamId = m.TeamId,
                    TeamName = m.Team != null ? m.Team.NameTeam : "",
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<MacrocycleEntity?> GetByIdAsync(string macrocycleId)
        {
            return await _context.Macrocycles
                .Include(m => m.Events)
                .Include(m => m.Periods)
                .Include(m => m.Mesocycles)
                .Include(m => m.Microcycles)
                .Include(m => m.Coach)
                .Include(m => m.Team)
                .FirstOrDefaultAsync(m => m.MacrocycleId == macrocycleId);
        }

        public async Task<bool> UpdateAsync(MacrocycleEntity macrocycle)
        {
            try
            {
                macrocycle.UpdatedAt = DateTime.Now;
                _context.Macrocycles.Update(macrocycle);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string macrocycleId)
        {
            try
            {
                var macrocycle = await _context.Macrocycles
                    .FirstOrDefaultAsync(m => m.MacrocycleId == macrocycleId);
                if (macrocycle == null) return false;

                // Delete child entities first
                var events = await _context.MacrocycleEvents.Where(e => e.MacrocycleId == macrocycleId).ToListAsync();
                _context.MacrocycleEvents.RemoveRange(events);

                var periods = await _context.MacrocyclePeriods.Where(p => p.MacrocycleId == macrocycleId).ToListAsync();
                _context.MacrocyclePeriods.RemoveRange(periods);

                var mesocycles = await _context.Mesocycles.Where(m => m.MacrocycleId == macrocycleId).ToListAsync();
                _context.Mesocycles.RemoveRange(mesocycles);

                var microcycles = await _context.Microcycles.Where(m => m.MacrocycleId == macrocycleId).ToListAsync();
                _context.Microcycles.RemoveRange(microcycles);

                _context.Macrocycles.Remove(macrocycle);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en DeleteAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<MacrocycleEvent> AddEventAsync(MacrocycleEvent macrocycleEvent)
        {
            await _context.MacrocycleEvents.AddAsync(macrocycleEvent);
            await _context.SaveChangesAsync();
            return macrocycleEvent;
        }

        public async Task<bool> UpdateEventAsync(MacrocycleEvent macrocycleEvent)
        {
            try
            {
                _context.MacrocycleEvents.Update(macrocycleEvent);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateEventAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteEventAsync(string eventId)
        {
            try
            {
                var evt = await _context.MacrocycleEvents.FirstOrDefaultAsync(e => e.MacrocycleEventId == eventId);
                if (evt == null) return false;
                _context.MacrocycleEvents.Remove(evt);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en DeleteEventAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<Microcycle?> GetMicrocycleByIdAsync(int microcycleId)
        {
            return await _context.Microcycles.FirstOrDefaultAsync(m => m.MicrocycleId == microcycleId);
        }

        public async Task<bool> UpdateMicrocycleAsync(Microcycle microcycle)
        {
            try
            {
                _context.Microcycles.Update(microcycle);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateMicrocycleAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<List<MacrocycleSummaryDto>> GetCoachMacrocyclesAsync(int coachId)
        {
            return await _context.Macrocycles
                .Include(m => m.Coach)
                .Include(m => m.Team)
                .Where(m => m.CoachId == coachId)
                .OrderByDescending(m => m.StartDate)
                .Select(m => new MacrocycleSummaryDto
                {
                    MacrocycleId = m.MacrocycleId,
                    Name = m.Name,
                    AthleteName = m.AthleteName,
                    AthleteId = m.AthleteId,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    EventCount = m.Events.Count,
                    CoachId = m.CoachId,
                    CoachName = m.Coach != null ? m.Coach.FirstName + " " + m.Coach.LastName : "",
                    TeamId = m.TeamId,
                    TeamName = m.Team != null ? m.Team.NameTeam : "",
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> ValidateNoOverlapAsync(int athleteId, DateTime startDate, DateTime endDate, string? excludeMacrocycleId = null)
        {
            var query = _context.Macrocycles
                .Where(m => m.AthleteId == athleteId && m.StartDate < endDate && m.EndDate > startDate);

            if (!string.IsNullOrEmpty(excludeMacrocycleId))
                query = query.Where(m => m.MacrocycleId != excludeMacrocycleId);

            return !await query.AnyAsync();
        }

        public async Task<MacrocycleEvent?> GetEventByIdAsync(string eventId)
        {
            return await _context.MacrocycleEvents.FirstOrDefaultAsync(e => e.MacrocycleEventId == eventId);
        }

        public async Task DeletePeriodsAsync(string macrocycleId)
        {
            var periods = await _context.MacrocyclePeriods.Where(p => p.MacrocycleId == macrocycleId).ToListAsync();
            _context.MacrocyclePeriods.RemoveRange(periods);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMesocyclesAsync(string macrocycleId)
        {
            var mesocycles = await _context.Mesocycles.Where(m => m.MacrocycleId == macrocycleId).ToListAsync();
            _context.Mesocycles.RemoveRange(mesocycles);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMicrocyclesAsync(string macrocycleId)
        {
            var microcycles = await _context.Microcycles.Where(m => m.MacrocycleId == macrocycleId).ToListAsync();
            _context.Microcycles.RemoveRange(microcycles);
            await _context.SaveChangesAsync();
        }

        public async Task AddPeriodsAsync(IEnumerable<MacrocyclePeriod> periods)
        {
            await _context.MacrocyclePeriods.AddRangeAsync(periods);
            await _context.SaveChangesAsync();
        }

        public async Task AddMesocyclesAsync(IEnumerable<Mesocycle> mesocycles)
        {
            await _context.Mesocycles.AddRangeAsync(mesocycles);
            await _context.SaveChangesAsync();
        }

        public async Task AddMicrocyclesAsync(IEnumerable<Microcycle> microcycles)
        {
            await _context.Microcycles.AddRangeAsync(microcycles);
            await _context.SaveChangesAsync();
        }
    }
}




