using BocciaCoaching.Models.DTO.AssessSaremas;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssessSaremasController : ControllerBase
    {
        private readonly IAssessSaremasService _service;

        public AssessSaremasController(IAssessSaremasService service)
        {
            _service = service;
        }

        /// <summary>
        /// Crear una nueva evaluación SAREMAS+
        /// </summary>
        [HttpPost("AddEvaluation")]
        public async Task<ActionResult<ResponseContract<ResponseAddSaremasDto>>> AddEvaluation(AddSaremasEvaluationDto dto)
        {
            var result = await _service.CreateEvaluation(dto);
            return Ok(result);
        }

        /// <summary>
        /// Asignar atletas a la evaluación SAREMAS+
        /// </summary>
        [HttpPost("AthletesToEvaluated")]
        public async Task<ActionResult<ResponseContract<SaremasAthleteEvaluation>>> AthletesToEvaluated(RequestAddAthleteToSaremasDto dto)
        {
            var result = await _service.AddAthleteToEvaluation(dto);
            return Ok(result);
        }

        /// <summary>
        /// Registrar un lanzamiento individual de SAREMAS+
        /// </summary>
        [HttpPost("AddDetailsToEvaluation")]
        public async Task<ActionResult<ResponseContract<bool>>> AddDetailsToEvaluation(RequestAddSaremasDetailDto dto)
        {
            var result = await _service.AddThrowDetail(dto);
            return Ok(result);
        }

        /// <summary>
        /// Obtener la evaluación SAREMAS+ activa para un equipo y coach
        /// </summary>
        [HttpGet("GetActiveEvaluation/{teamId}/{coachId}")]
        public async Task<ActionResult<ResponseContract<ActiveSaremasEvaluationDto>>> GetActiveEvaluation(int teamId, int coachId)
        {
            var result = await _service.GetActiveEvaluation(teamId, coachId);
            return Ok(result);
        }

        /// <summary>
        /// Actualizar el estado de una evaluación SAREMAS+
        /// </summary>
        [HttpPut("UpdateState")]
        public async Task<ActionResult<ResponseContract<bool>>> UpdateState(UpdateSaremasStateDto dto)
        {
            var result = await _service.UpdateState(dto);
            return Ok(result);
        }

        /// <summary>
        /// Cancelar una evaluación SAREMAS+
        /// </summary>
        [HttpPost("Cancel")]
        public async Task<ActionResult<ResponseContract<bool>>> Cancel(CancelSaremasDto dto)
        {
            var result = await _service.CancelEvaluation(dto);
            return Ok(result);
        }

        /// <summary>
        /// Obtener todas las evaluaciones SAREMAS+ de un equipo
        /// </summary>
        [HttpGet("GetTeamEvaluations/{teamId}")]
        public async Task<ActionResult<ResponseContract<List<SaremasEvaluationSummaryDto>>>> GetTeamEvaluations(int teamId)
        {
            var result = await _service.GetTeamEvaluations(teamId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener el detalle completo de una evaluación SAREMAS+
        /// </summary>
        [HttpGet("GetEvaluationDetails/{saremasEvalId}")]
        public async Task<ActionResult<ResponseContract<SaremasEvaluationDetailsDto>>> GetEvaluationDetails(int saremasEvalId)
        {
            var result = await _service.GetEvaluationDetails(saremasEvalId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener estadísticas desglosadas de una evaluación SAREMAS+
        /// </summary>
        [HttpGet("GetEvaluationStatistics/{saremasEvalId}")]
        public async Task<ActionResult<ResponseContract<SaremasStatisticsDto>>> GetEvaluationStatistics(int saremasEvalId)
        {
            var result = await _service.GetEvaluationStatistics(saremasEvalId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener el historial de evaluaciones SAREMAS+ de un atleta
        /// </summary>
        [HttpGet("GetAthleteHistory/{athleteId}")]
        public async Task<ActionResult<ResponseContract<SaremasAthleteHistoryDto>>> GetAthleteHistory(int athleteId)
        {
            var result = await _service.GetAthleteHistory(athleteId);
            return Ok(result);
        }
    }
}

