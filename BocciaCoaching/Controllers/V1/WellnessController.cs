using BocciaCoaching.Infrastructure.Api;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers.V1
{
    /// <summary>
    /// ES: Test diario de bienestar (Wellness) de los atletas — API REST v1.
    /// Evalúa Sueño, Estrés, Fatiga y Dolor Muscular en una escala de 1 a 7; un registro por atleta por día.
    /// </summary>
    [Route("api/v1/wellness")]
    [Tags("Wellness")]
    public sealed class WellnessController(IWellnessService wellnessService) : ApiControllerBase
    {
        private const string GetByIdRoute = "GetWellnessById";

        /// <summary>Registra el test diario de bienestar de un atleta (uno por día).</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> Create(AddDailyWellnessDto request, CancellationToken cancellationToken)
        {
            var result = await wellnessService.RegisterDailyWellnessAsync(request, cancellationToken);
            return CreatedFromResult(result, GetByIdRoute, new { dailyWellnessId = result.Data?.DailyWellnessId });
        }

        /// <summary>Obtiene un test de bienestar por su identificador.</summary>
        [HttpGet("{dailyWellnessId:guid}", Name = GetByIdRoute)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> GetById(Guid dailyWellnessId, CancellationToken cancellationToken)
            => FromResult(await wellnessService.GetWellnessByIdAsync(dailyWellnessId, cancellationToken));

        /// <summary>Actualiza un test de bienestar.</summary>
        [HttpPut("{dailyWellnessId:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> Update(
            Guid dailyWellnessId, UpdateDailyWellnessDto request, CancellationToken cancellationToken)
        {
            request.DailyWellnessId = dailyWellnessId; // la ruta es la fuente de verdad del identificador
            return FromResult(await wellnessService.UpdateDailyWellnessAsync(request, cancellationToken));
        }

        /// <summary>Historial de tests de bienestar de un atleta.</summary>
        [HttpGet("~/api/v1/athletes/{athleteId:guid}/wellness")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<WellnessAthleteHistoryDto>>> GetAthleteHistory(Guid athleteId, CancellationToken cancellationToken)
            => FromResult(await wellnessService.GetAthleteHistoryAsync(athleteId, cancellationToken));

        /// <summary>Test de bienestar del día actual de un atleta (404 si aún no lo registra).</summary>
        [HttpGet("~/api/v1/athletes/{athleteId:guid}/wellness/today")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> GetToday(Guid athleteId, CancellationToken cancellationToken)
            => FromResult(await wellnessService.GetTodayWellnessAsync(athleteId, cancellationToken));

        /// <summary>Test de bienestar de un atleta para una fecha (formato yyyy-MM-dd).</summary>
        [HttpGet("~/api/v1/athletes/{athleteId:guid}/wellness/{date:datetime}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> GetByDate(Guid athleteId, DateTime date, CancellationToken cancellationToken)
            => FromResult(await wellnessService.GetWellnessByDateAsync(athleteId, date, cancellationToken));

        /// <summary>Tests de bienestar de un equipo para una fecha (por defecto, hoy).</summary>
        [HttpGet("~/api/v1/teams/{teamId:guid}/wellness")]
        public async Task<ActionResult<ResponseContract<List<ResponseDailyWellnessDto>>>> GetTeamWellness(
            Guid teamId, [FromQuery] DateTime? date, CancellationToken cancellationToken)
            => FromResult(await wellnessService.GetTeamWellnessByDateAsync(teamId, date ?? DateTime.Now, cancellationToken));
    }
}
