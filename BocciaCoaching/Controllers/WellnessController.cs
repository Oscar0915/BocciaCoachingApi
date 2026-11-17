using BocciaCoaching.Infrastructure.Api;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    /// <summary>
    /// ES: API antigua del test diario de bienestar. Se mantiene por compatibilidad con el frontend
    /// actual (siempre HTTP 200 con <c>success</c> en el cuerpo). Usar <see cref="V1.WellnessController"/>.
    /// </summary>
    [ApiController]
    [LegacyApi]
    [Obsolete("Usar /api/v1/wellness")]
    [Route("api/[controller]")]
    public class WellnessController(IWellnessService service) : ControllerBase
    {
        /// <summary>Obsoleto: usar POST /api/v1/wellness</summary>
        [HttpPost("AddDailyWellness")]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> AddDailyWellness(AddDailyWellnessDto dto)
            => Ok(await service.RegisterDailyWellnessAsync(dto, HttpContext.RequestAborted));

        /// <summary>Obsoleto: usar PUT /api/v1/wellness/{dailyWellnessId}</summary>
        [HttpPut("UpdateDailyWellness")]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> UpdateDailyWellness(UpdateDailyWellnessDto dto)
            => Ok(await service.UpdateDailyWellnessAsync(dto, HttpContext.RequestAborted));

        /// <summary>Obsoleto: usar GET /api/v1/athletes/{athleteId}/wellness/today</summary>
        [HttpGet("GetTodayWellness/{athleteId}")]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> GetTodayWellness(Guid athleteId)
            => Ok(await service.GetTodayWellnessAsync(athleteId, HttpContext.RequestAborted));

        /// <summary>Obsoleto: usar GET /api/v1/athletes/{athleteId}/wellness/{date}</summary>
        [HttpGet("GetWellnessByDate/{athleteId}/{date}")]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> GetWellnessByDate(Guid athleteId, DateTime date)
            => Ok(await service.GetWellnessByDateAsync(athleteId, date, HttpContext.RequestAborted));

        /// <summary>Obsoleto: usar GET /api/v1/wellness/{dailyWellnessId}</summary>
        [HttpGet("GetWellnessById/{dailyWellnessId}")]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> GetWellnessById(Guid dailyWellnessId)
            => Ok(await service.GetWellnessByIdAsync(dailyWellnessId, HttpContext.RequestAborted));

        /// <summary>Obsoleto: usar GET /api/v1/athletes/{athleteId}/wellness</summary>
        [HttpGet("GetAthleteHistory/{athleteId}")]
        public async Task<ActionResult<ResponseContract<WellnessAthleteHistoryDto>>> GetAthleteHistory(Guid athleteId)
            => Ok(await service.GetAthleteHistoryAsync(athleteId, HttpContext.RequestAborted));

        /// <summary>Obsoleto: usar GET /api/v1/teams/{teamId}/wellness?date=</summary>
        [HttpGet("GetTeamWellnessByDate/{teamId}/{date}")]
        public async Task<ActionResult<ResponseContract<List<ResponseDailyWellnessDto>>>> GetTeamWellnessByDate(Guid teamId, DateTime date)
            => Ok(await service.GetTeamWellnessByDateAsync(teamId, date, HttpContext.RequestAborted));
    }
}
