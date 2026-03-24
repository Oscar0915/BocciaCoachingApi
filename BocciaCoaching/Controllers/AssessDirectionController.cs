using BocciaCoaching.Models.DTO.AssessDirection;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssessDirectionController : ControllerBase
    {
        private readonly IAssessDirectionService _assessDirectionService;

        public AssessDirectionController(IAssessDirectionService assessDirectionService)
        {
            _assessDirectionService = assessDirectionService;
        }

        /// <summary>
        /// Crear una nueva evaluación de control de dirección
        /// </summary>
        [HttpPost("AddEvaluation")]
        public async Task<ActionResult<ResponseContract<ResponseAddAssessDirectionDto>>> NewEvaluation(
            AddAssessDirectionDto dto)
        {
            var result = await _assessDirectionService.CreateEvaluation(dto);
            return Ok(result);
        }

        /// <summary>
        /// Agregar un atleta a la evaluación de dirección
        /// </summary>
        [HttpPost("AthletesToEvaluated")]
        public async Task<ActionResult<ResponseContract<AthletesToEvaluatedDirection>>> AthletesToEvaluated(
            RequestAddAthleteToDirectionEvaluationDto dto)
        {
            var result = await _assessDirectionService.AgregarAtletaAEvaluacion(dto);
            return Ok(result);
        }

        /// <summary>
        /// Agregar el detalle de un lanzamiento de la prueba de control de dirección.
        /// Son 24 lanzamientos: 8 a 3m, 8 a 6m, 8 a 9m.
        /// 4 desde box derecho y 4 desde box izquierdo por distancia.
        /// </summary>
        [HttpPost("AddDetailsToEvaluation")]
        public async Task<ActionResult<bool>> AddDetailsToEvaluation(
            RequestAddDetailToDirectionEvaluation dto)
        {
            var result = await _assessDirectionService.AgregarDetalleDeEvaluacion(dto);
            return Ok(result);
        }

        /// <summary>
        /// Valida si hay una evaluación de dirección activa y devuelve toda la información
        /// </summary>
        [HttpGet("GetActiveEvaluation/{teamId}/{coachId}")]
        public async Task<ActionResult<ResponseContract<ActiveDirectionEvaluationDto>>> GetActiveEvaluation(
            int teamId, int coachId)
        {
            var result = await _assessDirectionService.GetActiveEvaluationWithDetails(teamId, coachId);
            return Ok(result);
        }

        /// <summary>
        /// Endpoint de debugging para verificar datos de evaluaciones de dirección
        /// </summary>
        [HttpGet("DebugEvaluations/{teamId}")]
        public async Task<ActionResult> DebugEvaluations(int teamId)
        {
            var debugInfo = await _assessDirectionService.GetEvaluationDebugInfo(teamId);
            return Ok(debugInfo);
        }

        /// <summary>
        /// Actualizar el estado de una evaluación de dirección (A=Activa, T=Terminada, C=Cancelada)
        /// </summary>
        [HttpPut("UpdateState")]
        public async Task<ActionResult<ResponseContract<bool>>> UpdateEvaluationState(
            UpdateAssessDirectionDto updateDto)
        {
            var result = await _assessDirectionService.UpdateEvaluationState(updateDto);
            return Ok(result);
        }

        /// <summary>
        /// Cancela una evaluación de dirección (sólo el coach creador puede cancelar)
        /// </summary>
        [HttpPost("Cancel")]
        public async Task<ActionResult<ResponseContract<bool>>> CancelEvaluation(
            CancelAssessDirectionDto cancelDto)
        {
            var result = await _assessDirectionService.CancelEvaluation(cancelDto);
            return Ok(result);
        }

        /// <summary>
        /// Obtener todas las evaluaciones de dirección de un equipo
        /// </summary>
        [HttpGet("GetTeamEvaluations/{teamId}")]
        public async Task<ActionResult<ResponseContract<List<DirectionEvaluationSummaryDto>>>> GetTeamEvaluations(
            int teamId)
        {
            var result = await _assessDirectionService.GetTeamEvaluations(teamId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener estadísticas de una evaluación de dirección específica
        /// </summary>
        [HttpGet("GetEvaluationStatistics/{assessDirectionId}")]
        public async Task<ActionResult<ResponseContract<List<DirectionAthleteStatisticsDto>>>> GetEvaluationStatistics(
            int assessDirectionId)
        {
            var result = await _assessDirectionService.GetEvaluationStatistics(assessDirectionId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener los detalles completos de una evaluación de dirección específica
        /// </summary>
        [HttpGet("GetEvaluationDetails/{assessDirectionId}")]
        public async Task<ActionResult<ResponseContract<DirectionEvaluationDetailsDto>>> GetEvaluationDetails(
            int assessDirectionId)
        {
            var result = await _assessDirectionService.GetEvaluationDetails(assessDirectionId);
            return Ok(result);
        }
    }
}

