using BocciaCoaching.Models.Entities;
using TrainingSessionEntity = BocciaCoaching.Models.Entities.TrainingSession;

namespace BocciaCoaching.Repositories.Interfaces.ITrainingSession
{
    public interface ITrainingSessionRepository
    {
        Task<TrainingSessionEntity> CreateAsync(TrainingSessionEntity session);
        Task<TrainingSessionEntity?> GetByIdAsync(int sessionId);
        Task<List<TrainingSessionEntity>> GetByMicrocycleAsync(int microcycleId);
        Task<bool> UpdateAsync(TrainingSessionEntity session);
        Task<bool> DeleteAsync(int sessionId);

        // Session Sections
        Task<SessionSection?> GetSectionByIdAsync(int sectionId);
        Task<SessionSection> AddSectionAsync(SessionSection section);
        Task<bool> UpdateSectionAsync(SessionSection section);
        Task<bool> DeleteSectionAsync(int sectionId);

        // Session Parts
        Task<SessionPart?> GetPartByIdAsync(int partId);

        // Validations
        Task<bool> MicrocycleExistsAsync(int microcycleId);

        // Athlete queries
        Task<List<TrainingSessionEntity>> GetByAthleteAndDateRangeAsync(int athleteId, DateTime startDate, DateTime endDate);
        Task<bool> SessionBelongsToAthleteAsync(int sessionId, int athleteId);
    }
}
