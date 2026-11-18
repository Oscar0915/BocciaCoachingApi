using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.IWellness
{
    /// <summary>
    /// ES: Acceso a datos del cuestionario diario de bienestar. Devuelve datos (no ResponseContract):
    /// las reglas de negocio y los permisos viven en <c>WellnessService</c>.
    /// </summary>
    public interface IWellnessRepository
    {
        /// <summary>ES: Indica si el atleta ya registró el cuestionario para ese día.</summary>
        Task<bool> ExistsForDayAsync(Guid athleteId, DateTime day, CancellationToken cancellationToken = default);

        /// <summary>ES: Inserta un cuestionario. Lanza DbUpdateException si viola el índice único (atleta, día).</summary>
        Task AddAsync(DailyWellness entity, CancellationToken cancellationToken = default);

        /// <summary>ES: Entidad rastreada para modificarla y luego llamar a <see cref="SaveChangesAsync"/>.</summary>
        Task<DailyWellness?> GetForUpdateAsync(Guid dailyWellnessId, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<bool> AthleteExistsAsync(Guid athleteId, CancellationToken cancellationToken = default);

        Task<bool> TeamExistsAsync(Guid teamId, CancellationToken cancellationToken = default);

        /// <summary>ES: true si el usuario es entrenador (dueño o miembro con rol Coach) de algún equipo del atleta.</summary>
        Task<bool> IsCoachOfAthleteAsync(Guid coachId, Guid athleteId, CancellationToken cancellationToken = default);

        /// <summary>ES: true si el usuario es entrenador (dueño o miembro con rol Coach) del equipo.</summary>
        Task<bool> IsCoachOfTeamAsync(Guid coachId, Guid teamId, CancellationToken cancellationToken = default);

        Task<ResponseDailyWellnessDto?> GetByAthleteAndDateAsync(Guid athleteId, DateTime date, CancellationToken cancellationToken = default);

        Task<ResponseDailyWellnessDto?> GetByIdAsync(Guid dailyWellnessId, CancellationToken cancellationToken = default);

        /// <summary>ES: Historial del atleta (null si el atleta no existe).</summary>
        Task<WellnessAthleteHistoryDto?> GetAthleteHistoryAsync(Guid athleteId, CancellationToken cancellationToken = default);

        /// <summary>ES: Atletas activos del equipo (miembros con rol Atleta).</summary>
        Task<List<WellnessPendingAthleteDto>> GetTeamAthletesAsync(Guid teamId, CancellationToken cancellationToken = default);

        /// <summary>
        /// ES: Cuestionarios del día de los atletas del equipo (aunque los hayan registrado sin indicar equipo).
        /// </summary>
        Task<List<ResponseDailyWellnessDto>> GetTeamWellnessByDateAsync(Guid teamId, DateTime date, CancellationToken cancellationToken = default);
    }
}
