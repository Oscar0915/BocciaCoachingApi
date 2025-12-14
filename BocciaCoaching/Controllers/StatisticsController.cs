using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        /// <summary>
        /// Obtiene las estadísticas recientes de evaluación de fuerza para un coach y equipo específico
        /// </summary>
        /// <param name="coachId">ID del coach</param>
        /// <param name="teamId">ID del equipo</param>
        /// <returns>Lista de estadísticas recientes</returns>
        [HttpGet("RecentStrengthStats")]
        public async Task<ActionResult<ResponseContract<List<StrengthTestSummaryDto>>>> GetRecentStatistics(
            [FromQuery] int coachId, 
            [FromQuery] int teamId)
        {
            if (coachId <= 0 || teamId <= 0)
            {
                return BadRequest(ResponseContract<List<StrengthTestSummaryDto>>.Fail("Coach ID y Team ID son requeridos y deben ser válidos"));
            }

            var result = await _statisticsService.GetRecentStatistics(coachId, teamId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene todas las estadísticas de evaluación de fuerza filtradas por equipo, incluyendo promedios del equipo
        /// </summary>
        /// <param name="teamId">ID del equipo para filtrar las estadísticas</param>
        /// <returns>Estadísticas completas del equipo con información individual de cada atleta y promedios del equipo</returns>
        [HttpGet("TeamStrengthStats/{teamId}")]
        public async Task<ActionResult<ResponseContract<TeamStrengthStatisticsDto>>> GetTeamStrengthStatistics(int teamId)
        {
            if (teamId <= 0)
            {
                return BadRequest(ResponseContract<TeamStrengthStatisticsDto>.Fail("Team ID debe ser un valor válido mayor a 0"));
            }

            var result = await _statisticsService.GetTeamStrengthStatistics(teamId);
            
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Depuración: Obtiene información detallada sobre las evaluaciones de un equipo para diagnosticar problemas
        /// </summary>
        /// <param name="teamId">ID del equipo para depurar</param>
        /// <returns>Información detallada de evaluaciones, atletas y estadísticas del equipo</returns>
        [HttpGet("DebugTeamEvaluations/{teamId}")]
        public async Task<ActionResult<ResponseContract<object>>> GetTeamEvaluationsDebug(int teamId)
        {
            if (teamId <= 0)
            {
                return BadRequest(ResponseContract<object>.Fail("Team ID debe ser un valor válido mayor a 0"));
            }

            var result = await _statisticsService.GetTeamEvaluationsDebug(teamId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene estadísticas individualizadas por evaluación para un equipo, incluyendo evaluaciones no finalizadas
        /// </summary>
        /// <param name="teamId">ID del equipo para obtener estadísticas individualizadas</param>
        /// <returns>Estadísticas detalladas de cada evaluación individual del equipo</returns>
        [HttpGet("TeamStrengthStatsIndividualized/{teamId}")]
        public async Task<ActionResult<ResponseContract<TeamStrengthStatisticsDto>>> GetTeamStrengthStatisticsIndividualized(int teamId)
        {
            if (teamId <= 0)
            {
                return BadRequest(ResponseContract<TeamStrengthStatisticsDto>.Fail("Team ID debe ser un valor válido mayor a 0"));
            }

            var result = await _statisticsService.GetTeamStrengthStatisticsIndividualized(teamId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene estadísticas detalladas de un atleta específico
        /// </summary>
        /// <param name="athleteId">ID del atleta</param>
        /// <returns>Estadísticas detalladas del atleta</returns>
        [HttpGet("AthleteStats/{athleteId}")]
        public async Task<ActionResult<ResponseContract<List<StrengthTestSummaryDto>>>> GetAthleteStatistics(int athleteId)
        {
            if (athleteId <= 0)
            {
                return BadRequest(ResponseContract<List<StrengthTestSummaryDto>>.Fail("Athlete ID debe ser un valor válido mayor a 0"));
            }

            // Por ahora usamos el método existente con coachId = 0 (se puede modificar el repositorio después)
            var result = await _statisticsService.GetRecentStatistics(0, 0);
            
            // Filtrar por athleteId
            if (result.Success && result.Data != null && result.Data.Any())
            {
                var athleteStats = result.Data.Where(x => x.AthleteId == athleteId).ToList();
                return Ok(ResponseContract<List<StrengthTestSummaryDto>>.Ok(athleteStats, "Estadísticas del atleta obtenidas correctamente"));
            }

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el resumen de estadísticas de todos los equipos
        /// </summary>
        /// <returns>Resumen de estadísticas por equipo</returns>
        [HttpGet("AllTeamsStats")]
        public Task<ActionResult<ResponseContract<List<TeamStrengthStatisticsDto>>>> GetAllTeamsStatistics()
        {
            // Este método necesitaría una implementación específica en el servicio
            // Por ahora retornamos un mensaje indicando que no está implementado
            return Task.FromResult<ActionResult<ResponseContract<List<TeamStrengthStatisticsDto>>>>(
                Ok(ResponseContract<List<TeamStrengthStatisticsDto>>.Fail("Endpoint no implementado aún"))
            );
        }

        /// <summary>
        /// Obtiene estadísticas comparativas entre equipos
        /// </summary>
        /// <param name="teamIds">Lista de IDs de equipos para comparar</param>
        /// <returns>Estadísticas comparativas entre los equipos especificados</returns>
        [HttpPost("CompareTeams")]
        public async Task<ActionResult<ResponseContract<List<TeamStrengthStatisticsDto>>>> CompareTeamStatistics([FromBody] List<int> teamIds)
        {
            if (!teamIds.Any())
            {
                return BadRequest(ResponseContract<List<TeamStrengthStatisticsDto>>.Fail("Se requiere al menos un Team ID"));
            }

            var results = new List<TeamStrengthStatisticsDto>();

            foreach (var teamId in teamIds.Distinct())
            {
                var teamStats = await _statisticsService.GetTeamStrengthStatistics(teamId);
                if (teamStats.Success && teamStats.Data != null)
                {
                    results.Add(teamStats.Data);
                }
            }

            if (!results.Any())
            {
                return NotFound(ResponseContract<List<TeamStrengthStatisticsDto>>.Fail("No se encontraron estadísticas para los equipos especificados"));
            }

            return Ok(ResponseContract<List<TeamStrengthStatisticsDto>>.Ok(results, $"Estadísticas comparativas obtenidas para {results.Count} equipos"));
        }
    }
}
