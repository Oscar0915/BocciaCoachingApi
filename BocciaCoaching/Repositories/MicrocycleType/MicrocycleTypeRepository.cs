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
                .Include(m => m.DefaultDays)
                .Where(m => m.Status)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<Models.Entities.MicrocycleType?> GetByIdAsync(string id)
        {
            return await _context.MicrocycleTypes
                .Include(m => m.DefaultDays)
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

        public async Task<List<CoachMicrocycleTypeDay>> GetCoachDaysAsync(int coachId, string microcycleTypeId)
        {
            return await _context.CoachMicrocycleTypeDays
                .Where(c => c.CoachId == coachId && c.MicrocycleTypeId == microcycleTypeId)
                .ToListAsync();
        }

        public async Task SaveCoachDaysAsync(int coachId, string microcycleTypeId, List<CoachMicrocycleTypeDay> days)
        {
            // Eliminar días previos del coach para este tipo
            var existing = await _context.CoachMicrocycleTypeDays
                .Where(c => c.CoachId == coachId && c.MicrocycleTypeId == microcycleTypeId)
                .ToListAsync();
            _context.CoachMicrocycleTypeDays.RemoveRange(existing);

            // Insertar los nuevos
            _context.CoachMicrocycleTypeDays.AddRange(days);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ResetCoachDaysAsync(int coachId, string microcycleTypeId)
        {
            var existing = await _context.CoachMicrocycleTypeDays
                .Where(c => c.CoachId == coachId && c.MicrocycleTypeId == microcycleTypeId)
                .ToListAsync();
            if (!existing.Any()) return false;
            _context.CoachMicrocycleTypeDays.RemoveRange(existing);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

