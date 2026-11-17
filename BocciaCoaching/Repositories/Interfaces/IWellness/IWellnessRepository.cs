using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.IWellness
{
    /// <summary>
    /// ES: Acceso a datos del test diario de bienestar. Devuelve datos (no ResponseContract):
    /// las reglas de negocio viven en <c>WellnessService</c>.
    /// </summary>
    public interface IWellnessRepository
    {
        /// <summary>ES: Indica si el atleta ya registró el test para ese día.</summary>
        Task<bool> ExistsForDayAsync(Guid athleteId, DateTime day, CancellationToken cancellationToken = default);

        /// <summary>ES: Inserta un nuevo test. Lanza DbUpdateException si viola el índice único (atleta, día).</summary>
        Task AddAsync(DailyWellness entity, CancellationToken cancellationToken = default);

        /// <summary>ES: Obtiene la entidad rastreada para modificarla y luego llamar a <see cref="SaveChangesAsync"/>.</summary>
        Task<DailyWellness?> GetForUpdateAsync(Guid dailyWellnessId, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<bool> AthleteExistsAsync(Guid athleteId, CancellationToken cancellationToken = default);

        /// <summary>ES: Obtiene el test de bienestar de un atleta para una fecha específica.</summary>
        Task<ResponseDailyWellnessDto?> GetByAthleteAndDateAsync(Guid athleteId, DateTime date, CancellationToken cancellationToken = default);

        /// <summary>ES: Obtiene un test de bienestar por su identificador.</summary>
        Task<ResponseDailyWellnessDto?> GetByIdAsync(Guid dailyWellnessId, CancellationToken cancellationToken = default);

        /// <summary>ES: Historial de tests de un atleta (null si el atleta no existe).</summary>
        Task<WellnessAthleteHistoryDto?> GetAthleteHistoryAsync(Guid athleteId, CancellationToken cancellationToken = default);

        /// <summary>ES: Todos los tests de un equipo para una fecha.</summary>
        Task<List<ResponseDailyWellnessDto>> GetTeamWellnessByDateAsync(Guid teamId, DateTime date, CancellationToken cancellationToken = default);
    }
}
