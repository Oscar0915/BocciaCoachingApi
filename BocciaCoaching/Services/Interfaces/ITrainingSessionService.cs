using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Session;

namespace BocciaCoaching.Services.Interfaces
{
    public interface ITrainingSessionService
    {
        Task<ResponseContract<TrainingSessionResponseDto>> CreateSession(CreateTrainingSessionDto dto);
        Task<ResponseContract<TrainingSessionResponseDto>> GetById(int sessionId);
        Task<ResponseContract<List<TrainingSessionSummaryDto>>> GetByMicrocycle(int microcycleId);
        Task<ResponseContract<TrainingSessionResponseDto>> UpdateSession(UpdateTrainingSessionDto dto);
        Task<ResponseContract<bool>> DeleteSession(int sessionId);

        // Photo evidence
        Task<ResponseContract<TrainingSessionResponseDto>> UploadPhoto(int sessionId, int photoNumber, Microsoft.AspNetCore.Http.IFormFile file);

        // Session sections
        Task<ResponseContract<SessionSectionResponseDto>> AddSection(AddSessionSectionDto dto);
        Task<ResponseContract<SessionSectionResponseDto>> UpdateSection(UpdateSessionSectionDto dto);
        Task<ResponseContract<bool>> DeleteSection(int sectionId);

        // Athlete operations
        Task<ResponseContract<List<AthleteSessionSummaryDto>>> GetSessionsByAthleteInDateRange(GetAthleteSessionsDto dto);
        Task<ResponseContract<TrainingSessionResponseDto>> GetSessionDetailForAthlete(int sessionId, int athleteId);
        Task<ResponseContract<TrainingSessionResponseDto>> StartSession(AthleteUpdateSessionStatusDto dto);
        Task<ResponseContract<TrainingSessionResponseDto>> FinishSession(AthleteUpdateSessionStatusDto dto);
    }
}

