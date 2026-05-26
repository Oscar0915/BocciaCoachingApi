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

        /// <summary>Obtiene los tipos de microciclo configurados junto con los tipos que están construidos en la aplicación</summary>
        Task<ResponseContract<MicrocycleTypesOverviewDto>> GetOverview();

        /// <summary>Inserta un nuevo día por defecto para un tipo de microciclo</summary>
        Task<ResponseContract<MicrocycleTypeDayDefaultResponseDto>> CreateDayDefault(CreateMicrocycleTypeDayDefaultDto dto);

        // ─── Distribución personalizada por coach ───────────────────────────────

        /// <summary>
        /// Crea o actualiza la distribución de componentes de entrenamiento personalizada
        /// del coach para un tipo de microciclo específico (FísicaGeneral, Técnica, etc.).
        /// Solo afecta a ese coach.
        /// </summary>
        Task<ResponseContract<CoachMicrocycleTypeDistributionDto>> UpsertCoachDistribution(UpsertCoachMicrocycleTypeDistributionDto dto);

        /// <summary>Obtiene la distribución personalizada del coach para un tipo de microciclo</summary>
        Task<ResponseContract<CoachMicrocycleTypeDistributionDto?>> GetCoachDistribution(int coachId, string microcycleTypeId);

        /// <summary>Obtiene todas las distribuciones personalizadas del coach</summary>
        Task<ResponseContract<List<CoachMicrocycleTypeDistributionDto>>> GetAllCoachDistributions(int coachId);

        /// <summary>Elimina la distribución personalizada del coach y vuelve a los valores por defecto</summary>
        Task<ResponseContract<bool>> DeleteCoachDistribution(int coachId, string microcycleTypeId);
    }
}
