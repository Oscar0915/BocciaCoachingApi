using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Session;

namespace BocciaCoaching.Services.Interfaces
{
    public interface ITrainingSessionService
    {
        Task<ResponseContract<TrainingSessionResponseDto>> CreateSession(CreateTrainingSessionDto dto);
        Task<ResponseContract<TrainingSessionResponseDto>> GetById(Guid sessionId);
        Task<ResponseContract<List<TrainingSessionSummaryDto>>> GetByMicrocycle(Guid microcycleId);
        Task<ResponseContract<TrainingSessionResponseDto>> UpdateSession(UpdateTrainingSessionDto dto);
        Task<ResponseContract<bool>> DeleteSession(Guid sessionId);

        // Photo evidence
        Task<ResponseContract<TrainingSessionResponseDto>> UploadPhoto(Guid sessionId, int photoNumber, Microsoft.AspNetCore.Http.IFormFile file);

        // Session sections
        Task<ResponseContract<SessionSectionResponseDto>> AddSection(AddSessionSectionDto dto);
        Task<ResponseContract<SessionSectionResponseDto>> UpdateSection(UpdateSessionSectionDto dto);
        Task<ResponseContract<bool>> DeleteSection(Guid sectionId);

        // Athlete operations
        Task<ResponseContract<List<AthleteSessionSummaryDto>>> GetSessionsByAthleteInDateRange(GetAthleteSessionsDto dto);
        Task<ResponseContract<TrainingSessionResponseDto>> GetSessionDetailForAthlete(Guid sessionId, Guid athleteId);
        Task<ResponseContract<TrainingSessionResponseDto>> StartSession(AthleteUpdateSessionStatusDto dto);
        Task<ResponseContract<TrainingSessionResponseDto>> FinishSession(AthleteUpdateSessionStatusDto dto);
    }
}

