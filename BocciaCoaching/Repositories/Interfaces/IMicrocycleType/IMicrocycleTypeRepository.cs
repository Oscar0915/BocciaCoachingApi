using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.IMicrocycleType
{
    public interface IMicrocycleTypeRepository
    {
        Task<Models.Entities.MicrocycleType> CreateAsync(Models.Entities.MicrocycleType microcycleType);
        Task<List<Models.Entities.MicrocycleType>> GetAllAsync();
        Task<Models.Entities.MicrocycleType?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Models.Entities.MicrocycleType microcycleType);
        Task<bool> DeleteAsync(Guid id);

        // ─── Overrides de días por coach (CoachId != null en MicrocycleTypeDayDefault) ───

        /// <summary>Obtiene los overrides del coach para un tipo de microciclo</summary>
        Task<List<MicrocycleTypeDayDefault>> GetCoachDaysAsync(Guid coachId, Guid microcycleTypeId);

        /// <summary>Reemplaza todos los overrides del coach para un tipo (borra los viejos e inserta los nuevos)</summary>
        Task SaveCoachDaysAsync(Guid coachId, Guid microcycleTypeId, List<MicrocycleTypeDayDefault> days);

        /// <summary>Elimina todos los overrides del coach para un tipo (vuelve a los defaults del sistema)</summary>
        Task<bool> ResetCoachDaysAsync(Guid coachId, Guid microcycleTypeId);

        // ─── Distribución de componentes personalizada por coach ───────────────────────

        Task<CoachMicrocycleTypeDistribution?> GetCoachDistributionAsync(Guid coachId, Guid microcycleTypeId);
        Task<List<CoachMicrocycleTypeDistribution>> GetAllCoachDistributionsAsync(Guid coachId);
        Task UpsertCoachDistributionAsync(CoachMicrocycleTypeDistribution distribution);
        Task<bool> DeleteCoachDistributionAsync(Guid coachId, Guid microcycleTypeId);

        /// <summary>Conteo de microciclos construidos agrupados por tipo</summary>
        Task<Dictionary<string, int>> GetBuiltTypeSummaryAsync();

        /// <summary>Inserta un nuevo día por defecto global (CoachId == null) para un tipo</summary>
        Task<MicrocycleTypeDayDefault> CreateDayDefaultAsync(MicrocycleTypeDayDefault dayDefault);
    }
}
