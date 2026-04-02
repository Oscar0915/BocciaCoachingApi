using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.MicrocycleType;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MicrocycleTypeController : ControllerBase
    {
        private readonly IMicrocycleTypeService _service;

        public MicrocycleTypeController(IMicrocycleTypeService service)
        {
            _service = service;
        }

        /// <summary>Crear un nuevo tipo de microciclo (administrador)</summary>
        [HttpPost("Create")]
        public async Task<ActionResult<ResponseContract<MicrocycleTypeResponseDto>>> Create(CreateMicrocycleTypeDto dto)
        {
            var result = await _service.Create(dto);
            return Ok(result);
        }

        /// <summary>Obtener todos los tipos de microciclo (valores por defecto)</summary>
        [HttpGet("GetAll")]
        public async Task<ActionResult<ResponseContract<List<MicrocycleTypeResponseDto>>>> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        /// <summary>Obtener un tipo de microciclo por Id (valores por defecto)</summary>
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponseContract<MicrocycleTypeResponseDto>>> GetById(string id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }

        /// <summary>Obtener todos los tipos con los porcentajes personalizados del coach</summary>
        [HttpGet("GetAllForCoach/{coachId}")]
        public async Task<ActionResult<ResponseContract<List<MicrocycleTypeResponseDto>>>> GetAllForCoach(int coachId)
        {
            var result = await _service.GetAllForCoach(coachId);
            return Ok(result);
        }

        /// <summary>Obtener un tipo de microciclo con los porcentajes del coach</summary>
        [HttpGet("GetForCoach/{id}/{coachId}")]
        public async Task<ActionResult<ResponseContract<MicrocycleTypeResponseDto>>> GetForCoach(string id, int coachId)
        {
            var result = await _service.GetByIdForCoach(id, coachId);
            return Ok(result);
        }

        /// <summary>Actualizar los porcentajes personalizados de un coach</summary>
        [HttpPut("UpdateCoachPercentages")]
        public async Task<ActionResult<ResponseContract<bool>>> UpdateCoachPercentages(UpdateCoachPercentagesDto dto)
        {
            var result = await _service.UpdateCoachPercentages(dto);
            return Ok(result);
        }

        /// <summary>Restablecer los porcentajes de un coach a los valores por defecto</summary>
        [HttpDelete("ResetCoachPercentages/{coachId}/{microcycleTypeId}")]
        public async Task<ActionResult<ResponseContract<bool>>> ResetCoachPercentages(int coachId, string microcycleTypeId)
        {
            var result = await _service.ResetCoachPercentages(coachId, microcycleTypeId);
            return Ok(result);
        }
    }
}

