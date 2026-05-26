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

        /// <summary>
        /// Obtener los tipos de microciclo configurados (catálogo) y los tipos de microciclos
        /// que están construidos en macrociclos de la aplicación, con su cantidad de uso.
        /// </summary>
        [HttpGet("GetOverview")]
        public async Task<ActionResult<ResponseContract<MicrocycleTypesOverviewDto>>> GetOverview()
        {
            var result = await _service.GetOverview();
            return Ok(result);
        }

        /// <summary>Insertar un nuevo día por defecto para un tipo de microciclo</summary>
        [HttpPost("CreateDayDefault")]
        public async Task<ActionResult<ResponseContract<MicrocycleTypeDayDefaultResponseDto>>> CreateDayDefault(CreateMicrocycleTypeDayDefaultDto dto)
        {
            var result = await _service.CreateDayDefault(dto);
            return Ok(result);
        }

        // ─── Distribución de componentes personalizada por coach ─────────────────

        /// <summary>
        /// Guardar (crear o actualizar) la distribución de componentes de entrenamiento
        /// personalizada del coach para un tipo de microciclo.
        /// Los valores (FísicaGeneral, FísicaEspecial, Técnica, Táctica, Teórica, Psicológica)
        /// deben sumar 1.0. Solo afecta a ese coach específico.
        /// </summary>
        [HttpPut("UpsertCoachDistribution")]
        public async Task<ActionResult<ResponseContract<CoachMicrocycleTypeDistributionDto>>> UpsertCoachDistribution(
            UpsertCoachMicrocycleTypeDistributionDto dto)
        {
            var result = await _service.UpsertCoachDistribution(dto);
            return Ok(result);
        }

        /// <summary>
        /// Obtener la distribución personalizada del coach para un tipo de microciclo.
        /// Si no tiene personalización devuelve null (se usan los valores por defecto del sistema).
        /// </summary>
        [HttpGet("GetCoachDistribution/{coachId}/{microcycleTypeId}")]
        public async Task<ActionResult<ResponseContract<CoachMicrocycleTypeDistributionDto?>>> GetCoachDistribution(
            int coachId, string microcycleTypeId)
        {
            var result = await _service.GetCoachDistribution(coachId, microcycleTypeId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener todas las distribuciones personalizadas de un coach
        /// (una por cada tipo de microciclo que haya configurado).
        /// </summary>
        [HttpGet("GetAllCoachDistributions/{coachId}")]
        public async Task<ActionResult<ResponseContract<List<CoachMicrocycleTypeDistributionDto>>>> GetAllCoachDistributions(int coachId)
        {
            var result = await _service.GetAllCoachDistributions(coachId);
            return Ok(result);
        }

        /// <summary>
        /// Eliminar la distribución personalizada del coach para un tipo de microciclo.
        /// Los nuevos macrociclos de ese coach usarán los valores por defecto del sistema.
        /// </summary>
        [HttpDelete("DeleteCoachDistribution/{coachId}/{microcycleTypeId}")]
        public async Task<ActionResult<ResponseContract<bool>>> DeleteCoachDistribution(int coachId, string microcycleTypeId)
        {
            var result = await _service.DeleteCoachDistribution(coachId, microcycleTypeId);
            return Ok(result);
        }
    }
}

