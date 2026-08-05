using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Wellness;

namespace BocciaCoaching.Repositories.Interfaces.IWellness
{
    public interface IWellnessRepository
    {
        /// <summary>
        /// ES: Registra el test diario de bienestar si el atleta no lo ha respondido ese día.
        /// </summary>
        Task<ResponseContract<ResponseDailyWellnessDto>> CreateIfNotExistsForDayAsync(AddDailyWellnessDto dto);

        /// <summary>
        /// ES: Actualiza un test diario de bienestar existente.
        /// </summary>
        Task<ResponseContract<ResponseDailyWellnessDto>> UpdateAsync(UpdateDailyWellnessDto dto);

        /// <summary>
        /// ES: Obtiene el test de bienestar de un atleta para una fecha específica.
        /// </summary>
        Task<ResponseDailyWellnessDto?> GetByAthleteAndDateAsync(Guid athleteId, DateTime date);

        /// <summary>
        /// ES: Obtiene un test de bienestar por su identificador.
        /// </summary>
        Task<ResponseDailyWellnessDto?> GetByIdAsync(Guid dailyWellnessId);

        /// <summary>
        /// ES: Obtiene el historial de tests de bienestar de un atleta.
        /// </summary>
        Task<WellnessAthleteHistoryDto?> GetAthleteHistoryAsync(Guid athleteId);

        /// <summary>
        /// ES: Obtiene todos los tests de bienestar de un equipo para una fecha.
        /// </summary>
        Task<List<ResponseDailyWellnessDto>> GetTeamWellnessByDateAsync(Guid teamId, DateTime date);
    }
}

