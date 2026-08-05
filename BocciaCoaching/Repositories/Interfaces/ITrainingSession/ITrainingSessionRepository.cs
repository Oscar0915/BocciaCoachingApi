using BocciaCoaching.Models.Entities;
using TrainingSessionEntity = BocciaCoaching.Models.Entities.TrainingSession;

namespace BocciaCoaching.Repositories.Interfaces.ITrainingSession
{
    public interface ITrainingSessionRepository
    {
        Task<TrainingSessionEntity> CreateAsync(TrainingSessionEntity session);
        Task<TrainingSessionEntity?> GetByIdAsync(Guid sessionId);
        Task<List<TrainingSessionEntity>> GetByMicrocycleAsync(Guid microcycleId);
        Task<bool> UpdateAsync(TrainingSessionEntity session);
        Task<bool> DeleteAsync(Guid sessionId);

        // Session Sections
        Task<SessionSection?> GetSectionByIdAsync(Guid sectionId);
        Task<SessionSection> AddSectionAsync(SessionSection section);
        Task<bool> UpdateSectionAsync(SessionSection section);
        Task<bool> DeleteSectionAsync(Guid sectionId);

        // Session Parts
        Task<SessionPart?> GetPartByIdAsync(Guid partId);

        // Validations
        Task<bool> MicrocycleExistsAsync(Guid microcycleId);

        // Athlete queries
        Task<List<TrainingSessionEntity>> GetByAthleteAndDateRangeAsync(Guid athleteId, DateTime startDate, DateTime endDate);
        Task<bool> SessionBelongsToAthleteAsync(Guid sessionId, Guid athleteId);
    }
}
