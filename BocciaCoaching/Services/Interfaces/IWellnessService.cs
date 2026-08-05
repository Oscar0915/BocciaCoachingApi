using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Wellness;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IWellnessService
    {
        Task<ResponseContract<ResponseDailyWellnessDto>> RegisterDailyWellness(AddDailyWellnessDto dto);
        Task<ResponseContract<ResponseDailyWellnessDto>> UpdateDailyWellness(UpdateDailyWellnessDto dto);
        Task<ResponseContract<ResponseDailyWellnessDto>> GetTodayWellness(Guid athleteId);
        Task<ResponseContract<ResponseDailyWellnessDto>> GetWellnessByDate(Guid athleteId, DateTime date);
        Task<ResponseContract<ResponseDailyWellnessDto>> GetWellnessById(Guid dailyWellnessId);
        Task<ResponseContract<WellnessAthleteHistoryDto>> GetAthleteHistory(Guid athleteId);
        Task<ResponseContract<List<ResponseDailyWellnessDto>>> GetTeamWellnessByDate(Guid teamId, DateTime date);
    }
}

