using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Macrocycle;
using BocciaCoaching.Models.Entities;
using MacrocycleEntity = BocciaCoaching.Models.Entities.Macrocycle;

namespace BocciaCoaching.Repositories.Interfaces.IMacrocycle
{
    public interface IMacrocycleRepository
    {
        Task<MacrocycleEntity> CreateAsync(MacrocycleEntity macrocycle);
        Task<List<MacrocycleSummaryDto>> GetByAthleteAsync(int athleteId);
        Task<List<MacrocycleSummaryDto>> GetByTeamAsync(int teamId);
        Task<MacrocycleEntity?> GetByIdAsync(string macrocycleId);
        Task<bool> UpdateAsync(MacrocycleEntity macrocycle);
        Task<bool> DeleteAsync(string macrocycleId);
        Task<MacrocycleEvent> AddEventAsync(MacrocycleEvent macrocycleEvent);
        Task<bool> UpdateEventAsync(MacrocycleEvent macrocycleEvent);
        Task<bool> DeleteEventAsync(string eventId);
        Task<Microcycle?> GetMicrocycleByIdAsync(int microcycleId);
        Task<bool> UpdateMicrocycleAsync(Microcycle microcycle);
        Task<List<MacrocycleSummaryDto>> GetCoachMacrocyclesAsync(int coachId);
        Task<bool> ValidateNoOverlapAsync(int athleteId, DateTime startDate, DateTime endDate, string? excludeMacrocycleId = null);
        Task<MacrocycleEvent?> GetEventByIdAsync(string eventId);

        // Bulk operations for recalculation
        Task DeletePeriodsAsync(string macrocycleId);
        Task DeleteMesocyclesAsync(string macrocycleId);
        Task DeleteMicrocyclesAsync(string macrocycleId);
        Task AddPeriodsAsync(IEnumerable<MacrocyclePeriod> periods);
        Task AddMesocyclesAsync(IEnumerable<Mesocycle> mesocycles);
        Task AddMicrocyclesAsync(IEnumerable<Microcycle> microcycles);
    }
}
