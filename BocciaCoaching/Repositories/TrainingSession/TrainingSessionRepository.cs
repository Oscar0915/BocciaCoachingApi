using BocciaCoaching.Data;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.ITrainingSession;
using Microsoft.EntityFrameworkCore;
using TrainingSessionEntity = BocciaCoaching.Models.Entities.TrainingSession;

namespace BocciaCoaching.Repositories.TrainingSession
{
    public class TrainingSessionRepository : ITrainingSessionRepository
    {
        private readonly ApplicationDbContext _context;

        public TrainingSessionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TrainingSessionEntity> CreateAsync(TrainingSessionEntity session)
        {
            await _context.TrainingSessions.AddAsync(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<TrainingSessionEntity?> GetByIdAsync(int sessionId)
        {
            return await _context.TrainingSessions
                .Include(s => s.Parts)
                    .ThenInclude(p => p.Sections)
                .FirstOrDefaultAsync(s => s.TrainingSessionId == sessionId);
        }

        public async Task<List<TrainingSessionEntity>> GetByMicrocycleAsync(int microcycleId)
        {
            return await _context.TrainingSessions
                .Include(s => s.Parts)
                    .ThenInclude(p => p.Sections)
                .Where(s => s.MicrocycleId == microcycleId)
                .OrderBy(s => s.DayOfWeek)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(TrainingSessionEntity session)
        {
            try
            {
                session.UpdatedAt = DateTime.Now;
                _context.TrainingSessions.Update(session);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en TrainingSession UpdateAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int sessionId)
        {
            try
            {
                var session = await _context.TrainingSessions
                    .Include(s => s.Parts)
                        .ThenInclude(p => p.Sections)
                    .FirstOrDefaultAsync(s => s.TrainingSessionId == sessionId);

                if (session == null) return false;

                // Remove sections first, then parts, then session
                foreach (var part in session.Parts)
                {
                    _context.SessionSections.RemoveRange(part.Sections);
                }
                _context.SessionParts.RemoveRange(session.Parts);
                _context.TrainingSessions.Remove(session);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en TrainingSession DeleteAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<SessionSection?> GetSectionByIdAsync(int sectionId)
        {
            return await _context.SessionSections
                .Include(s => s.SessionPart)
                .FirstOrDefaultAsync(s => s.SessionSectionId == sectionId);
        }

        public async Task<SessionSection> AddSectionAsync(SessionSection section)
        {
            await _context.SessionSections.AddAsync(section);
            await _context.SaveChangesAsync();
            return section;
        }

        public async Task<bool> UpdateSectionAsync(SessionSection section)
        {
            try
            {
                section.UpdatedAt = DateTime.Now;
                _context.SessionSections.Update(section);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateSectionAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteSectionAsync(int sectionId)
        {
            try
            {
                var section = await _context.SessionSections.FindAsync(sectionId);
                if (section == null) return false;

                _context.SessionSections.Remove(section);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en DeleteSectionAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<SessionPart?> GetPartByIdAsync(int partId)
        {
            return await _context.SessionParts
                .Include(p => p.Sections)
                .FirstOrDefaultAsync(p => p.SessionPartId == partId);
        }

        public async Task<bool> MicrocycleExistsAsync(int microcycleId)
        {
            return await _context.Microcycles.AnyAsync(m => m.MicrocycleId == microcycleId);
        }
    }
}
