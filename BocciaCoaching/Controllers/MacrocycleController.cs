using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Macrocycle;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MacrocycleController : ControllerBase
    {
        private readonly IMacrocycleService _service;

        public MacrocycleController(IMacrocycleService service)
        {
            _service = service;
        }

        /// <summary>
        /// Crear un macrociclo completo (con eventos). El backend calcula períodos, mesociclos y microciclos.
        /// </summary>
        [HttpPost("Create")]
        public async Task<ActionResult<ResponseContract<MacrocycleResponseDto>>> Create(CreateMacrocycleDto dto)
        {
            var result = await _service.CreateMacrocycle(dto);
            return Ok(result);
        }

        /// <summary>
        /// Obtener todos los macrociclos de un atleta
        /// </summary>
        [HttpGet("GetByAthlete/{athleteId}")]
        public async Task<ActionResult<ResponseContract<List<MacrocycleSummaryDto>>>> GetByAthlete(int athleteId)
        {
            var result = await _service.GetByAthlete(athleteId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener todos los macrociclos de un equipo
        /// </summary>
        [HttpGet("GetByTeam/{teamId}")]
        public async Task<ActionResult<ResponseContract<List<MacrocycleSummaryDto>>>> GetByTeam(int teamId)
        {
            var result = await _service.GetByTeam(teamId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener un macrociclo completo con todas sus sub-entidades
        /// </summary>
        [HttpGet("GetById/{macrocycleId}")]
        public async Task<ActionResult<ResponseContract<MacrocycleResponseDto>>> GetById(string macrocycleId)
        {
            var result = await _service.GetById(macrocycleId);
            return Ok(result);
        }

        /// <summary>
        /// Actualizar un macrociclo (recalcula períodos, mesociclos y microciclos)
        /// </summary>
        [HttpPut("Update")]
        public async Task<ActionResult<ResponseContract<MacrocycleResponseDto>>> Update(UpdateMacrocycleDto dto)
        {
            var result = await _service.UpdateMacrocycle(dto);
            return Ok(result);
        }

        /// <summary>
        /// Eliminar un macrociclo y todas sus sub-entidades
        /// </summary>
        [HttpDelete("Delete/{macrocycleId}")]
        public async Task<ActionResult<ResponseContract<bool>>> Delete(string macrocycleId)
        {
            var result = await _service.DeleteMacrocycle(macrocycleId);
            return Ok(result);
        }

        /// <summary>
        /// Agregar un evento al macrociclo y recalcular estructura
        /// </summary>
        [HttpPost("AddEvent")]
        public async Task<ActionResult<ResponseContract<MacrocycleResponseDto>>> AddEvent(AddMacrocycleEventDto dto)
        {
            var result = await _service.AddEvent(dto);
            return Ok(result);
        }

        /// <summary>
        /// Actualizar un evento existente y recalcular estructura
        /// </summary>
        [HttpPut("UpdateEvent")]
        public async Task<ActionResult<ResponseContract<MacrocycleResponseDto>>> UpdateEvent(UpdateMacrocycleEventDto dto)
        {
            var result = await _service.UpdateEvent(dto);
            return Ok(result);
        }

        /// <summary>
        /// Eliminar un evento y recalcular estructura
        /// </summary>
        [HttpDelete("DeleteEvent/{eventId}")]
        public async Task<ActionResult<ResponseContract<MacrocycleResponseDto>>> DeleteEvent(string eventId)
        {
            var result = await _service.DeleteEvent(eventId);
            return Ok(result);
        }

        /// <summary>
        /// Actualizar un microciclo individual (distribución de entrenamiento, pico de rendimiento)
        /// </summary>
        [HttpPut("UpdateMicrocycle")]
        public async Task<ActionResult<ResponseContract<bool>>> UpdateMicrocycle(UpdateMicrocycleDto dto)
        {
            var result = await _service.UpdateMicrocycle(dto);
            return Ok(result);
        }

        /// <summary>
        /// Obtener todos los macrociclos creados por un coach
        /// </summary>
        [HttpGet("GetCoachMacrocycles/{coachId}")]
        public async Task<ActionResult<ResponseContract<List<MacrocycleSummaryDto>>>> GetCoachMacrocycles(int coachId)
        {
            var result = await _service.GetCoachMacrocycles(coachId);
            return Ok(result);
        }

        /// <summary>
        /// Duplicar un macrociclo para otro atleta o período
        /// </summary>
        [HttpPost("Duplicate/{macrocycleId}")]
        public async Task<ActionResult<ResponseContract<MacrocycleResponseDto>>> Duplicate(string macrocycleId, DuplicateMacrocycleDto dto)
        {
            var result = await _service.DuplicateMacrocycle(macrocycleId, dto);
            return Ok(result);
        }
    }
}

