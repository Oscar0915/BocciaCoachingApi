using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.MicrocycleType;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IMicrocycleTypeService
    {
        Task<ResponseContract<MicrocycleTypeResponseDto>> Create(CreateMicrocycleTypeDto dto);
        Task<ResponseContract<List<MicrocycleTypeResponseDto>>> GetAll();
        Task<ResponseContract<MicrocycleTypeResponseDto>> GetById(string id);
        Task<ResponseContract<MicrocycleTypeResponseDto>> GetByIdForCoach(string id, int coachId);
        Task<ResponseContract<List<MicrocycleTypeResponseDto>>> GetAllForCoach(int coachId);
        Task<ResponseContract<bool>> UpdateCoachPercentages(UpdateCoachPercentagesDto dto);
        Task<ResponseContract<bool>> ResetCoachPercentages(int coachId, string microcycleTypeId);
    }
}

