﻿﻿using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
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
        /// <param name="coachId">ID del entrenador para buscar evaluación activa</param>
        /// <returns>Información completa de la evaluación activa o null si no hay ninguna</returns>
        [HttpGet("GetActiveEvaluation/{teamId}/{coachId}")]
        public async Task<ActionResult<ResponseContract<ActiveEvaluationDto>>> GetActiveEvaluation(int teamId, int coachId)
        {
            var result = await _assessStrengthService.GetActiveEvaluationWithDetails(teamId, coachId);
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

        /// <summary>
        /// Actualizar el estado de una evaluación (A=Activa, T=Terminada, C=Cancelada)
        /// </summary>
        /// <param name="updateDto">Datos para actualizar el estado</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("UpdateState")]
        public async Task<ActionResult<ResponseContract<bool>>> UpdateEvaluationState(UpdateAssessStregthDto updateDto)
        {
            var result = await _assessStrengthService.UpdateEvaluationState(updateDto);
            return Ok(result);
        }

        /// <summary>
        /// Obtener todas las evaluaciones de un equipo (activas e históricas)
        /// </summary>
        /// <param name="teamId">ID del equipo</param>
        /// <returns>Lista de evaluaciones del equipo</returns>
        [HttpGet("GetTeamEvaluations/{teamId}")]
        public async Task<ActionResult<ResponseContract<List<EvaluationSummaryDto>>>> GetTeamEvaluations(int teamId)
        {
            var result = await _assessStrengthService.GetTeamEvaluations(teamId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener estadísticas de una evaluación específica
        /// </summary>
        /// <param name="assessStrengthId">ID de la evaluación</param>
        /// <returns>Estadísticas de la evaluación</returns>
        [HttpGet("GetEvaluationStatistics/{assessStrengthId}")]
        public async Task<ActionResult<ResponseContract<List<AthleteStatisticsDto>>>> GetEvaluationStatistics(int assessStrengthId)
        {
            var result = await _assessStrengthService.GetEvaluationStatistics(assessStrengthId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener los detalles completos de una evaluación específica
        /// </summary>
        /// <param name="assessStrengthId">ID de la evaluación</param>
        /// <returns>Detalles completos de la evaluación</returns>
        [HttpGet("GetEvaluationDetails/{assessStrengthId}")]
        public async Task<ActionResult<ResponseContract<EvaluationDetailsDto>>> GetEvaluationDetails(int assessStrengthId)
        {
            var result = await _assessStrengthService.GetEvaluationDetails(assessStrengthId);
            return Ok(result);
        }
    }
}
