using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Services.Wellness;

namespace BocciaCoaching.Services.Interfaces
{
    /// <summary>
    /// ES: Cuestionario diario de bienestar (McLean 2010). Todas las operaciones reciben quién las pide
    /// (<see cref="WellnessRequester"/>) para aplicar permisos y calcular "hoy" en su zona horaria.
    /// Permisos: el atleta ve y registra lo suyo; el entrenador, lo de los atletas de sus equipos; el admin, todo.
    /// </summary>
    public interface IWellnessService
    {
        WellnessQuestionnaireDto GetQuestionnaire();

        /// <summary>ES: ¿Debe y ya respondió hoy el usuario autenticado? Lo consulta la app al ingresar.</summary>
        Task<ResponseContract<WellnessTodayStatusDto>> GetMyTodayStatusAsync(WellnessRequester requester, CancellationToken cancellationToken = default);

        /// <summary>ES: El atleta autenticado registra su cuestionario de hoy.</summary>
        Task<ResponseContract<ResponseDailyWellnessDto>> SubmitMyWellnessAsync(WellnessRequester requester, SubmitMyWellnessDto dto, CancellationToken cancellationToken = default);

        Task<ResponseContract<ResponseDailyWellnessDto>> RegisterDailyWellnessAsync(WellnessRequester requester, AddDailyWellnessDto dto, CancellationToken cancellationToken = default);
        Task<ResponseContract<ResponseDailyWellnessDto>> UpdateDailyWellnessAsync(WellnessRequester requester, UpdateDailyWellnessDto dto, CancellationToken cancellationToken = default);
        Task<ResponseContract<ResponseDailyWellnessDto>> GetTodayWellnessAsync(WellnessRequester requester, Guid athleteId, CancellationToken cancellationToken = default);
        Task<ResponseContract<ResponseDailyWellnessDto>> GetWellnessByDateAsync(WellnessRequester requester, Guid athleteId, DateTime date, CancellationToken cancellationToken = default);
        Task<ResponseContract<ResponseDailyWellnessDto>> GetWellnessByIdAsync(WellnessRequester requester, Guid dailyWellnessId, CancellationToken cancellationToken = default);
        Task<ResponseContract<WellnessAthleteHistoryDto>> GetAthleteHistoryAsync(WellnessRequester requester, Guid athleteId, CancellationToken cancellationToken = default);
        Task<ResponseContract<List<ResponseDailyWellnessDto>>> GetTeamWellnessByDateAsync(WellnessRequester requester, Guid teamId, DateTime? date, CancellationToken cancellationToken = default);

        /// <summary>ES: Resumen del día para el entrenador: semáforos, rojos primero y atletas pendientes.</summary>
        Task<ResponseContract<TeamWellnessSummaryDto>> GetTeamSummaryAsync(WellnessRequester requester, Guid teamId, DateTime? date, CancellationToken cancellationToken = default);
    }
}
