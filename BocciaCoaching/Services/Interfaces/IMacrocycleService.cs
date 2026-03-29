using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Macrocycle;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IMacrocycleService
    {
        Task<ResponseContract<MacrocycleResponseDto>> CreateMacrocycle(CreateMacrocycleDto dto);
        Task<ResponseContract<List<MacrocycleSummaryDto>>> GetByAthlete(int athleteId);
        Task<ResponseContract<List<MacrocycleSummaryDto>>> GetByTeam(int teamId);
        Task<ResponseContract<MacrocycleResponseDto>> GetById(string macrocycleId);
        Task<ResponseContract<MacrocycleResponseDto>> UpdateMacrocycle(UpdateMacrocycleDto dto);
        Task<ResponseContract<bool>> DeleteMacrocycle(string macrocycleId);
        Task<ResponseContract<MacrocycleResponseDto>> AddEvent(AddMacrocycleEventDto dto);
        Task<ResponseContract<MacrocycleResponseDto>> UpdateEvent(UpdateMacrocycleEventDto dto);
        Task<ResponseContract<MacrocycleResponseDto>> DeleteEvent(string eventId);
        Task<ResponseContract<bool>> UpdateMicrocycle(UpdateMicrocycleDto dto);
        Task<ResponseContract<List<MacrocycleSummaryDto>>> GetCoachMacrocycles(int coachId);
        Task<ResponseContract<MacrocycleResponseDto>> DuplicateMacrocycle(string macrocycleId, DuplicateMacrocycleDto dto);
    }
}

