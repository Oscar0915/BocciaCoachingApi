using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Rol;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly IRolService _service;

        public RolController(IRolService service)
        {
            _service = service;
        }

        /// <summary>Crear un nuevo rol</summary>
        [HttpPost("Create")]
        public async Task<ActionResult<ResponseContract<RolResponseDto>>> Create(CreateRolDto dto)
        {
            var result = await _service.Create(dto);
            return Ok(result);
        }

        /// <summary>Obtener todos los roles</summary>
        [HttpGet("GetAll")]
        public async Task<ActionResult<ResponseContract<List<RolResponseDto>>>> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        /// <summary>Obtener un rol por Id</summary>
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponseContract<RolResponseDto>>> GetById(Guid id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }

        /// <summary>Actualizar un rol existente</summary>
        [HttpPut("Update")]
        public async Task<ActionResult<ResponseContract<RolResponseDto>>> Update(UpdateRolDto dto)
        {
            var result = await _service.Update(dto);
            return Ok(result);
        }

        /// <summary>Eliminar un rol</summary>
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<ResponseContract<bool>>> Delete(Guid id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }
    }
}

