using BocciaCoaching.Data;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IMicrocycleType;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.MicrocycleType
{
    public class MicrocycleTypeRepository : IMicrocycleTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public MicrocycleTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Entities.MicrocycleType> CreateAsync(Models.Entities.MicrocycleType microcycleType)
        {
            _context.MicrocycleTypes.Add(microcycleType);
            await _context.SaveChangesAsync();
            return microcycleType;
        }

        public async Task<List<Models.Entities.MicrocycleType>> GetAllAsync()
        {
            return await _context.MicrocycleTypes
                .Include(m => m.DayConfigs)
                .Where(m => m.Status)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<Models.Entities.MicrocycleType?> GetByIdAsync(string id)
        {
            return await _context.MicrocycleTypes
                .Include(m => m.DayConfigs)
                .FirstOrDefaultAsync(m => m.MicrocycleTypeId == id);
        }

        public async Task<bool> UpdateAsync(Models.Entities.MicrocycleType microcycleType)
        {
            _context.MicrocycleTypes.Update(microcycleType);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.MicrocycleTypes.FindAsync(id);
            if (entity == null) return false;
            entity.Status = false;
            entity.UpdatedAt = DateTime.Now;
            return await _context.SaveChangesAsync() > 0;
        }

        // ─── Overrides de días por coach ──────────────────────────────────────────────

        public async Task<List<MicrocycleTypeDayDefault>> GetCoachDaysAsync(int coachId, string microcycleTypeId)
        {
            return await _context.MicrocycleTypeDayDefaults
                .Where(d => d.CoachId == coachId && d.MicrocycleTypeId == microcycleTypeId)
                .ToListAsync();
        }

        public async Task SaveCoachDaysAsync(int coachId, string microcycleTypeId, List<MicrocycleTypeDayDefault> days)
        {
            var existing = await _context.MicrocycleTypeDayDefaults
                .Where(d => d.CoachId == coachId && d.MicrocycleTypeId == microcycleTypeId)
                .ToListAsync();
            _context.MicrocycleTypeDayDefaults.RemoveRange(existing);
            _context.MicrocycleTypeDayDefaults.AddRange(days);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ResetCoachDaysAsync(int coachId, string microcycleTypeId)
        {
            var existing = await _context.MicrocycleTypeDayDefaults
                .Where(d => d.CoachId == coachId && d.MicrocycleTypeId == microcycleTypeId)
                .ToListAsync();
            if (!existing.Any()) return false;
            _context.MicrocycleTypeDayDefaults.RemoveRange(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        // ─── CoachMicrocycleTypeDistribution ──────────────────────────────────────────

        public async Task<CoachMicrocycleTypeDistribution?> GetCoachDistributionAsync(int coachId, string microcycleTypeId)
        {
            return await _context.CoachMicrocycleTypeDistributions
                .FirstOrDefaultAsync(d => d.CoachId == coachId && d.MicrocycleTypeId == microcycleTypeId);
        }

        public async Task<List<CoachMicrocycleTypeDistribution>> GetAllCoachDistributionsAsync(int coachId)
        {
            return await _context.CoachMicrocycleTypeDistributions
                .Include(d => d.MicrocycleType)
                .Where(d => d.CoachId == coachId)
                .ToListAsync();
        }

        public async Task UpsertCoachDistributionAsync(CoachMicrocycleTypeDistribution distribution)
        {
            var existing = await _context.CoachMicrocycleTypeDistributions
                .FirstOrDefaultAsync(d => d.CoachId == distribution.CoachId
                                       && d.MicrocycleTypeId == distribution.MicrocycleTypeId);
            if (existing == null)
            {
                _context.CoachMicrocycleTypeDistributions.Add(distribution);
            }
            else
            {
                existing.FisicaGeneral  = distribution.FisicaGeneral;
                existing.FisicaEspecial = distribution.FisicaEspecial;
                existing.Tecnica        = distribution.Tecnica;
                existing.Tactica        = distribution.Tactica;
                existing.Teorica        = distribution.Teorica;
                existing.Psicologica    = distribution.Psicologica;
                existing.UpdatedAt      = DateTime.Now;
                _context.CoachMicrocycleTypeDistributions.Update(existing);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCoachDistributionAsync(int coachId, string microcycleTypeId)
        {
            var existing = await _context.CoachMicrocycleTypeDistributions
                .FirstOrDefaultAsync(d => d.CoachId == coachId && d.MicrocycleTypeId == microcycleTypeId);
            if (existing == null) return false;
            _context.CoachMicrocycleTypeDistributions.Remove(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Dictionary<string, int>> GetBuiltTypeSummaryAsync()
        {
            return await _context.Microcycles
                .Where(m => !string.IsNullOrEmpty(m.Type))
                .GroupBy(m => m.Type)
                .Select(g => new { TypeName = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.TypeName, x => x.Count);
        }

        public async Task<MicrocycleTypeDayDefault> CreateDayDefaultAsync(MicrocycleTypeDayDefault dayDefault)
        {
            _context.MicrocycleTypeDayDefaults.Add(dayDefault);
            await _context.SaveChangesAsync();
            return dayDefault;
        }
    }
}
