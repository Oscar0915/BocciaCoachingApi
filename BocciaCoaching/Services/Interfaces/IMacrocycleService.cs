using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Macrocycle;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IMacrocycleService
    {
        Task<ResponseContract<MacrocycleResponseDto>> CreateMacrocycle(CreateMacrocycleDto dto);
        Task<ResponseContract<List<MacrocycleSummaryDto>>> GetByAthlete(Guid athleteId);
        Task<ResponseContract<List<MacrocycleSummaryDto>>> GetByTeam(Guid teamId);
        Task<ResponseContract<MacrocycleResponseDto>> GetById(Guid macrocycleId);
        Task<ResponseContract<MacrocycleResponseDto>> UpdateMacrocycle(UpdateMacrocycleDto dto);
        Task<ResponseContract<bool>> DeleteMacrocycle(Guid macrocycleId);
        Task<ResponseContract<MacrocycleResponseDto>> AddEvent(AddMacrocycleEventDto dto);
        Task<ResponseContract<MacrocycleResponseDto>> UpdateEvent(UpdateMacrocycleEventDto dto);
        Task<ResponseContract<MacrocycleResponseDto>> DeleteEvent(Guid eventId);
        Task<ResponseContract<bool>> UpdateMicrocycle(UpdateMicrocycleDto dto);
        Task<ResponseContract<List<MacrocycleSummaryDto>>> GetCoachMacrocycles(Guid coachId);
        Task<ResponseContract<MacrocycleResponseDto>> DuplicateMacrocycle(Guid macrocycleId, DuplicateMacrocycleDto dto);

        /// <summary>
        /// Actualiza los días (porcentajes de lanzamiento por día de la semana) de un microciclo concreto.
        /// Reemplaza completamente los días actuales.
        /// </summary>
        Task<ResponseContract<bool>> UpdateMicycleDays(UpdateMicycleDaysDto dto);
    }
}

