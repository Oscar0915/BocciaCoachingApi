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
        public async Task<ActionResult<IEnumerable<ResponseContract<ResponseAddAssessStrengthDto>>>> NewEvaluation(AddAssessStrengthDto user)
        {
            var users = await _assessStrengthService.CreateEvaluation(user);
            return Ok(users);
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
    }
}
