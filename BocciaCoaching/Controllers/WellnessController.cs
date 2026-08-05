using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    /// <summary>
    /// ES: API para el test diario de bienestar (Wellness) de los atletas.
    /// Evalúa Sueño, Estrés, Fatiga y Dolor Muscular en una escala de 1 a 7.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WellnessController : ControllerBase
    {
        private readonly IWellnessService _service;

        public WellnessController(IWellnessService service)
        {
            _service = service;
        }

        /// <summary>
        /// Registrar el test diario de bienestar de un atleta (uno por día).
        /// </summary>
        [HttpPost("AddDailyWellness")]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> AddDailyWellness(AddDailyWellnessDto dto)
        {
            var result = await _service.RegisterDailyWellness(dto);
            return Ok(result);
        }

        /// <summary>
        /// Actualizar el test diario de bienestar de un atleta.
        /// </summary>
        [HttpPut("UpdateDailyWellness")]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> UpdateDailyWellness(UpdateDailyWellnessDto dto)
        {
            var result = await _service.UpdateDailyWellness(dto);
            return Ok(result);
        }

        /// <summary>
        /// Obtener el test de bienestar del día actual para un atleta.
        /// </summary>
        [HttpGet("GetTodayWellness/{athleteId}")]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> GetTodayWellness(Guid athleteId)
        {
            var result = await _service.GetTodayWellness(athleteId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener el test de bienestar de un atleta para una fecha específica.
        /// </summary>
        [HttpGet("GetWellnessByDate/{athleteId}/{date}")]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> GetWellnessByDate(Guid athleteId, DateTime date)
        {
            var result = await _service.GetWellnessByDate(athleteId, date);
            return Ok(result);
        }

        /// <summary>
        /// Obtener un test de bienestar por su identificador.
        /// </summary>
        [HttpGet("GetWellnessById/{dailyWellnessId}")]
        public async Task<ActionResult<ResponseContract<ResponseDailyWellnessDto>>> GetWellnessById(Guid dailyWellnessId)
        {
            var result = await _service.GetWellnessById(dailyWellnessId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener el historial de tests de bienestar de un atleta.
        /// </summary>
        [HttpGet("GetAthleteHistory/{athleteId}")]
        public async Task<ActionResult<ResponseContract<WellnessAthleteHistoryDto>>> GetAthleteHistory(Guid athleteId)
        {
            var result = await _service.GetAthleteHistory(athleteId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener todos los tests de bienestar de un equipo para una fecha.
        /// </summary>
        [HttpGet("GetTeamWellnessByDate/{teamId}/{date}")]
        public async Task<ActionResult<ResponseContract<List<ResponseDailyWellnessDto>>>> GetTeamWellnessByDate(Guid teamId, DateTime date)
        {
            var result = await _service.GetTeamWellnessByDate(teamId, date);
            return Ok(result);
        }
    }
}

