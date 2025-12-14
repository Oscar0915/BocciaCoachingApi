using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssessStrengthController: ControllerBase
    {

        private readonly IAssessStrengthService _assessStrengthService;

        public AssessStrengthController(IAssessStrengthService assessStrengthService) { 
        _assessStrengthService = assessStrengthService;
        }



        [HttpPost("AddEvaluation")]
        public async Task<ActionResult<ResponseContract<ResponseAddAssessStrengthDto>>> NewEvaluation(AddAssessStrengthDto user)
        {
            var result = await _assessStrengthService.CreateEvaluation(user);
            return Ok(result);
        }


        [HttpPost("AthletesToEvaluated")]
        public async Task<ActionResult<IEnumerable<ResponseContract<AthletesToEvaluated>>>> AthletesToEvaluated(RequestAddAthleteToEvaluationDto user)
        {
            var users = await _assessStrengthService.AgregarAtletaAEvaluacion(user);
            return Ok(users);
        }


        /// <summary>
        /// Agregar el detalle de un lanzamiento de la prueba de fuerza
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("AddDeatilsToEvaluation")]
        public async Task<ActionResult<IEnumerable<bool>>> AddDeatilsToEvaluation(RequestAddDetailToEvaluationForAthlete user)
        {
            var users = await _assessStrengthService.AgregarDetalleDeEvaluacion(user);
            return Ok(users);
        }

        /// <summary>
        /// Valida si hay una evaluación de fuerza activa y devuelve toda la información de la evaluación con todos los lanzamientos
        /// </summary>
        /// <param name="teamId">ID del equipo para buscar evaluación activa</param>
        /// <returns>Información completa de la evaluación activa o null si no hay ninguna</returns>
        [HttpGet("GetActiveEvaluation/{teamId}")]
        public async Task<ActionResult<ResponseContract<ActiveEvaluationDto>>> GetActiveEvaluation(int teamId)
        {
            var result = await _assessStrengthService.GetActiveEvaluationWithDetails(teamId);
            return Ok(result);
        }

        /// <summary>
        /// Endpoint de debugging para verificar qué datos hay en las tablas relacionadas con evaluaciones
        /// </summary>
        /// <param name="teamId">ID del equipo</param>
        /// <returns>Información de debugging sobre las evaluaciones del equipo</returns>
        [HttpGet("DebugEvaluations/{teamId}")]
        public async Task<ActionResult> DebugEvaluations(int teamId)
        {
            var debugInfo = await _assessStrengthService.GetEvaluationDebugInfo(teamId);
            return Ok(debugInfo);
        }
    }
}
