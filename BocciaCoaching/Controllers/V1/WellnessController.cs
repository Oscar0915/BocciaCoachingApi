using BocciaCoaching.Infrastructure.Api;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Services.Interfaces;
using BocciaCoaching.Services.Wellness;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BocciaCoaching.Controllers.V1
{
    /// <summary>
    /// ES: Cuestionario diario de bienestar (McLean 2010) — API REST v1.
    /// 5 ítems de 1 (pésimo) a 5 (excelente); semáforo Verde/Amarillo/Rojo con veto por ítem ≤ 2.
    /// "Hoy" se calcula en la zona horaria de la cabecera <c>X-Time-Zone</c> (IANA, p. ej. America/Bogota)
    /// o en la configurada por defecto.
    /// </summary>
    [Route("api/v1/wellness")]
    [Tags("Wellness")]
    public sealed class WellnessController(IWellnessService wellnessService, IOptions<WellnessOptions> options) : ApiControllerBase
    {
        private const string GetByIdRoute = "GetWellnessById";

        private WellnessRequester Requester => WellnessRequester.From(User, Request, options.Value);

        /// <summary>Preguntas y opciones del cuestionario, para pintarlo en la app.</summary>
        [HttpGet("questionnaire")]
        public ActionResult<ResponseContract<WellnessQuestionnaireDto>> GetQuestionnaire()
            => FromResult(ResponseContract<WellnessQuestionnaireDto>.Ok(wellnessService.GetQuestionnaire()));

        /// <summary>
        /// Lo consulta la app al ingresar: si <c>mustComplete</c> es true, debe mostrar el cuestionario.
        /// Si ya lo respondió hoy, incluye el resultado con su semáforo y retroalimentación.
        /// </summary>
        [HttpGet("me/today")]
        public async Task<ActionResult<ResponseContract<WellnessTodayStatusDto>>> GetMyTodayStatus(CancellationToken cancellationToken)
            => FromResult(await wellnessService.GetMyTodayStatusAsync(Requester, cancellationToken));

        /// <summary>El atleta autenticado registra su cuestionario de hoy. Devuelve el semáforo y los mensajes.</summary>
        [HttpPost("me")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> SubmitMine(SubmitMyWellnessDto request, CancellationToken cancellationToken)
        {
            var result = await wellnessService.SubmitMyWellnessAsync(Requester, request, cancellationToken);
            return CreatedFromResult(result, GetByIdRoute, new { dailyWellnessId = result.Data?.DailyWellnessId });
        }

        /// <summary>
        /// Registra el cuestionario de un atleta. El atleta solo el suyo (de hoy); el entrenador, el de atletas
        /// de sus equipos (también días pasados); el admin, cualquiera.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> Create(AddDailyWellnessDto request, CancellationToken cancellationToken)
        {
            var result = await wellnessService.RegisterDailyWellnessAsync(Requester, request, cancellationToken);
            return CreatedFromResult(result, GetByIdRoute, new { dailyWellnessId = result.Data?.DailyWellnessId });
        }

        /// <summary>Obtiene un cuestionario por su identificador.</summary>
        [HttpGet("{dailyWellnessId:guid}", Name = GetByIdRoute)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> GetById(Guid dailyWellnessId, CancellationToken cancellationToken)
            => FromResult(await wellnessService.GetWellnessByIdAsync(Requester, dailyWellnessId, cancellationToken));

        /// <summary>Corrige un cuestionario (el atleta, solo el de hoy). Recalcula el semáforo.</summary>
        [HttpPut("{dailyWellnessId:guid}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> Update(
            Guid dailyWellnessId, UpdateDailyWellnessDto request, CancellationToken cancellationToken)
        {
            request.DailyWellnessId = dailyWellnessId; // la ruta es la fuente de verdad del identificador
            return FromResult(await wellnessService.UpdateDailyWellnessAsync(Requester, request, cancellationToken));
        }

        /// <summary>Historial del atleta con días en verde, amarillo y rojo.</summary>
        [HttpGet("~/api/v1/athletes/{athleteId:guid}/wellness")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<WellnessAthleteHistoryDto>>> GetAthleteHistory(Guid athleteId, CancellationToken cancellationToken)
            => FromResult(await wellnessService.GetAthleteHistoryAsync(Requester, athleteId, cancellationToken));

        /// <summary>Cuestionario de hoy de un atleta (404 si aún no lo registra).</summary>
        [HttpGet("~/api/v1/athletes/{athleteId:guid}/wellness/today")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> GetToday(Guid athleteId, CancellationToken cancellationToken)
            => FromResult(await wellnessService.GetTodayWellnessAsync(Requester, athleteId, cancellationToken));

        /// <summary>Cuestionario de un atleta para una fecha (formato yyyy-MM-dd).</summary>
        [HttpGet("~/api/v1/athletes/{athleteId:guid}/wellness/{date:datetime}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> GetByDate(Guid athleteId, DateTime date, CancellationToken cancellationToken)
            => FromResult(await wellnessService.GetWellnessByDateAsync(Requester, athleteId, date, cancellationToken));

        /// <summary>Cuestionarios del equipo para una fecha (por defecto, hoy). Solo el entrenador del equipo.</summary>
        [HttpGet("~/api/v1/teams/{teamId:guid}/wellness")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<List<ResponseDailyWellnessDto>>>> GetTeamWellness(
            Guid teamId, [FromQuery] DateTime? date, CancellationToken cancellationToken)
            => FromResult(await wellnessService.GetTeamWellnessByDateAsync(Requester, teamId, date, cancellationToken));

        /// <summary>
        /// Resumen del día para el entrenador: cuántos en verde/amarillo/rojo, los rojos primero y quién falta.
        /// </summary>
        [HttpGet("~/api/v1/teams/{teamId:guid}/wellness/summary")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<TeamWellnessSummaryDto>>> GetTeamSummary(
            Guid teamId, [FromQuery] DateTime? date, CancellationToken cancellationToken)
            => FromResult(await wellnessService.GetTeamSummaryAsync(Requester, teamId, date, cancellationToken));
    }
}
