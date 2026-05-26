using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.IMicrocycleType
{
    public interface IMicrocycleTypeRepository
    {
        Task<Models.Entities.MicrocycleType> CreateAsync(Models.Entities.MicrocycleType microcycleType);
        Task<List<Models.Entities.MicrocycleType>> GetAllAsync();
        Task<Models.Entities.MicrocycleType?> GetByIdAsync(string id);
        Task<bool> UpdateAsync(Models.Entities.MicrocycleType microcycleType);
        Task<bool> DeleteAsync(string id);

        // Coach customization
        Task<List<CoachMicrocycleTypeDay>> GetCoachDaysAsync(int coachId, string microcycleTypeId);
        Task SaveCoachDaysAsync(int coachId, string microcycleTypeId, List<CoachMicrocycleTypeDay> days);
        Task<bool> ResetCoachDaysAsync(int coachId, string microcycleTypeId);

        /// <summary>Obtiene el conteo de microciclos construidos agrupados por tipo (campo Type de la tabla Microcycle)</summary>
        Task<Dictionary<string, int>> GetBuiltTypeSummaryAsync();

        /// <summary>Inserta un nuevo día por defecto asociado a un tipo de microciclo</summary>
        Task<MicrocycleTypeDayDefault> CreateDayDefaultAsync(MicrocycleTypeDayDefault dayDefault);
    }
}
