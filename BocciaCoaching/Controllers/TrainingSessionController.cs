using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Session;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingSessionController : ControllerBase
    {
        private readonly ITrainingSessionService _service;

        public TrainingSessionController(ITrainingSessionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Crear una sesión de entrenamiento para un microciclo.
        /// Si no se envían partes, se crean automáticamente las 4 partes predeterminadas
        /// (Propulsion, Saremas, 2x1, Escenarios de juego).
        /// </summary>
        [HttpPost("Create")]
        public async Task<ActionResult<ResponseContract<TrainingSessionResponseDto>>> Create(CreateTrainingSessionDto dto)
        {
            var result = await _service.CreateSession(dto);
            return Ok(result);
        }

        /// <summary>
        /// Obtener una sesión de entrenamiento por ID con todas sus partes y secciones
        /// </summary>
        [HttpGet("GetById/{sessionId}")]
        public async Task<ActionResult<ResponseContract<TrainingSessionResponseDto>>> GetById(int sessionId)
        {
            var result = await _service.GetById(sessionId);
            return Ok(result);
        }

        /// <summary>
        /// Obtener todas las sesiones de un microciclo (resumen)
        /// </summary>
        [HttpGet("GetByMicrocycle/{microcycleId}")]
        public async Task<ActionResult<ResponseContract<List<TrainingSessionSummaryDto>>>> GetByMicrocycle(int microcycleId)
        {
            var result = await _service.GetByMicrocycle(microcycleId);
            return Ok(result);
        }

        /// <summary>
        /// Actualizar una sesión de entrenamiento (estado, tiempos, porcentaje, día)
        /// </summary>
        [HttpPut("Update")]
        public async Task<ActionResult<ResponseContract<TrainingSessionResponseDto>>> Update(UpdateTrainingSessionDto dto)
        {
            var result = await _service.UpdateSession(dto);
            return Ok(result);
        }

        /// <summary>
        /// Eliminar una sesión de entrenamiento y todas sus partes/secciones
        /// </summary>
        [HttpDelete("Delete/{sessionId}")]
        public async Task<ActionResult<ResponseContract<bool>>> Delete(int sessionId)
        {
            var result = await _service.DeleteSession(sessionId);
            return Ok(result);
        }

    

        /// <summary>
        /// Agregar una sección a una parte de la sesión
        /// </summary>
        [HttpPost("AddSection")]
        public async Task<ActionResult<ResponseContract<SessionSectionResponseDto>>> AddSection(AddSessionSectionDto dto)
        {
            var result = await _service.AddSection(dto);
            return Ok(result);
        }

        /// <summary>
        /// Actualizar una sección individual
        /// </summary>
        [HttpPut("UpdateSection")]
        public async Task<ActionResult<ResponseContract<SessionSectionResponseDto>>> UpdateSection(UpdateSessionSectionDto dto)
        {
            var result = await _service.UpdateSection(dto);
            return Ok(result);
        }

        /// <summary>
        /// Eliminar una sección
        /// </summary>
        [HttpDelete("DeleteSection/{sectionId}")]
        public async Task<ActionResult<ResponseContract<bool>>> DeleteSection(int sectionId)
        {
            var result = await _service.DeleteSection(sectionId);
            return Ok(result);
        }

        // ===================== ATHLETE ENDPOINTS =====================

        /// <summary>
        /// Obtener las sesiones de entrenamiento asignadas a un atleta en un rango de fechas.
        /// Busca a través de la relación Macrocycle (AthleteId) → Microcycle (StartDate/EndDate) → TrainingSession.
        /// </summary>
        [HttpPost("Athlete/GetSessionsByDateRange")]
        public async Task<ActionResult<ResponseContract<List<AthleteSessionSummaryDto>>>> GetSessionsByAthleteInDateRange(GetAthleteSessionsDto dto)
        {
            var result = await _service.GetSessionsByAthleteInDateRange(dto);
            return Ok(result);
        }

        /// <summary>
        /// Obtener el detalle completo de una sesión con todas sus partes y secciones para un atleta.
        /// Valida que la sesión pertenezca al atleta.
        /// </summary>
        [HttpGet("Athlete/GetSessionDetail/{sessionId}/{athleteId}")]
        public async Task<ActionResult<ResponseContract<TrainingSessionResponseDto>>> GetSessionDetailForAthlete(int sessionId, int athleteId)
        {
            var result = await _service.GetSessionDetailForAthlete(sessionId, athleteId);
            return Ok(result);
        }

        /// <summary>
        /// Iniciar una sesión de entrenamiento (cambia estado de 'programada' a 'en_proceso' y registra la hora de inicio).
        /// Solo el atleta asignado puede iniciar la sesión.
        /// </summary>
        [HttpPut("Athlete/StartSession")]
        public async Task<ActionResult<ResponseContract<TrainingSessionResponseDto>>> StartSession(AthleteUpdateSessionStatusDto dto)
        {
            var result = await _service.StartSession(dto);
            return Ok(result);
        }

        /// <summary>
        /// Finalizar una sesión de entrenamiento (cambia estado de 'en_proceso' a 'finalizada' y registra la hora de fin).
        /// Solo el atleta asignado puede finalizar la sesión.
        /// </summary>
        [HttpPut("Athlete/FinishSession")]
        public async Task<ActionResult<ResponseContract<TrainingSessionResponseDto>>> FinishSession(AthleteUpdateSessionStatusDto dto)
        {
            var result = await _service.FinishSession(dto);
            return Ok(result);
        }
    }
}

