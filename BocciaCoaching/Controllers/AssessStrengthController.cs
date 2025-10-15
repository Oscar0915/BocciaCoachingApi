﻿using BocciaCoaching.Models.DTO.AssessStrength;
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
        public async Task<ActionResult<IEnumerable<bool>>> NewEvaluation(AddAssessStrengthDto user)
        {
            var users = await _assessStrengthService.CrearEvaluacion(user);
            return Ok(users);
        }


        [HttpPost("AthletesToEvaluated")]
        public async Task<ActionResult<IEnumerable<bool>>> AthletesToEvaluated(RequestAddAthleteToEvaluationDto user)
        {
            var users = await _assessStrengthService.AgregarAtletaAEvaluacion(user);
            return Ok(users);
        }


        [HttpPost("AddDeatilsToEvaluation")]
        public async Task<ActionResult<IEnumerable<bool>>> AddDeatilsToEvaluation(RequestAddDetailToEvaluationForAthlete user)
        {
            var users = await _assessStrengthService.AgregarDetalleDeEvaluacion(user);
            return Ok(users);
        }
    }
}
