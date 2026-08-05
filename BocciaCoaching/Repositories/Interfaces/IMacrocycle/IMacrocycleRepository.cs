using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Macrocycle;
using BocciaCoaching.Models.Entities;
using MacrocycleEntity = BocciaCoaching.Models.Entities.Macrocycle;

namespace BocciaCoaching.Repositories.Interfaces.IMacrocycle
{
    public interface IMacrocycleRepository
    {
        Task<MacrocycleEntity> CreateAsync(MacrocycleEntity macrocycle);
        Task<List<MacrocycleSummaryDto>> GetByAthleteAsync(Guid athleteId);
        Task<List<MacrocycleSummaryDto>> GetByTeamAsync(Guid teamId);
        Task<MacrocycleEntity?> GetByIdAsync(Guid macrocycleId);
        Task<bool> UpdateAsync(MacrocycleEntity macrocycle);
        Task<bool> DeleteAsync(Guid macrocycleId);
        Task<MacrocycleEvent> AddEventAsync(MacrocycleEvent macrocycleEvent);
        Task<bool> UpdateEventAsync(MacrocycleEvent macrocycleEvent);
        Task<bool> DeleteEventAsync(Guid eventId);
        Task<Microcycle?> GetMicrocycleByIdAsync(Guid microcycleId);
        Task<bool> UpdateMicrocycleAsync(Microcycle microcycle);
        Task<List<MacrocycleSummaryDto>> GetCoachMacrocyclesAsync(Guid coachId);
        Task<bool> ValidateNoOverlapAsync(Guid athleteId, DateTime startDate, DateTime endDate, Guid? excludeMacrocycleId = null);
        Task<MacrocycleEvent?> GetEventByIdAsync(Guid eventId);

        // Bulk operations for recalculation
        Task DeletePeriodsAsync(Guid macrocycleId);
        Task DeleteMesocyclesAsync(Guid macrocycleId);
        Task DeleteMicrocyclesAsync(Guid macrocycleId);
        Task AddPeriodsAsync(IEnumerable<MacrocyclePeriod> periods);
        Task AddMesocyclesAsync(IEnumerable<Mesocycle> mesocycles);
        Task AddMicrocyclesAsync(IEnumerable<Microcycle> microcycles);

        // Microcycle days operations
        /// <summary>Obtiene los días de un microciclo concreto</summary>
        Task<List<MicrocycleDay>> GetMicycleDaysAsync(Guid microcycleId);

        /// <summary>
        /// Reemplaza todos los días de un microciclo concreto.
        /// Elimina los días existentes e inserta los nuevos.
        /// </summary>
        Task SaveMicycleDaysAsync(Guid microcycleId, List<MicrocycleDay> days);
    }
}
