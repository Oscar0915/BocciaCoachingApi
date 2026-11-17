using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Wellness;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IWellnessService
    {
        Task<ResponseContract<ResponseDailyWellnessDto>> RegisterDailyWellnessAsync(AddDailyWellnessDto dto, CancellationToken cancellationToken = default);
        Task<ResponseContract<ResponseDailyWellnessDto>> UpdateDailyWellnessAsync(UpdateDailyWellnessDto dto, CancellationToken cancellationToken = default);
        Task<ResponseContract<ResponseDailyWellnessDto>> GetTodayWellnessAsync(Guid athleteId, CancellationToken cancellationToken = default);
        Task<ResponseContract<ResponseDailyWellnessDto>> GetWellnessByDateAsync(Guid athleteId, DateTime date, CancellationToken cancellationToken = default);
        Task<ResponseContract<ResponseDailyWellnessDto>> GetWellnessByIdAsync(Guid dailyWellnessId, CancellationToken cancellationToken = default);
        Task<ResponseContract<WellnessAthleteHistoryDto>> GetAthleteHistoryAsync(Guid athleteId, CancellationToken cancellationToken = default);
        Task<ResponseContract<List<ResponseDailyWellnessDto>>> GetTeamWellnessByDateAsync(Guid teamId, DateTime date, CancellationToken cancellationToken = default);
    }
}
