using BocciaCoaching.Infrastructure.Api;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    /// <summary>
    /// ES: API antigua de equipos. Se mantiene por compatibilidad con el frontend actual
    /// (siempre HTTP 200 con <c>success</c> en el cuerpo). Usar <see cref="V1.TeamsController"/>.
    /// </summary>
    [ApiController]
    [LegacyApi]
    [Obsolete("Usar /api/v1/teams")]
    [Route("api/[controller]")]
    public class TeamController(ITeamService teamService, ILogger<TeamController> logger) : ControllerBase
    {
        /// <summary>Obsoleto: usar POST /api/v1/teams</summary>
        [HttpPost("AddNewTeam")]
        public async Task<ActionResult<ResponseContract<Guid>>> Add(RequestTeamDto requestTeamDto)
            => Ok(await teamService.CreateTeamAsync(requestTeamDto, HttpContext.RequestAborted));

        /// <summary>Obsoleto: usar POST /api/v1/teams/{teamId}/members</summary>
        [HttpPost("AddNewTeamMember")]
        public async Task<ActionResult<ResponseContract<bool>>> AddMember(RequestTeamMemberDto requestTeamMemberDto)
            => Ok(await teamService.AddTeamMemberAsync(requestTeamMemberDto.TeamId, requestTeamMemberDto.UserId, HttpContext.RequestAborted));

        /// <summary>Obsoleto: usar GET /api/v1/users/{userId}/teams</summary>
        [HttpGet("GetTeamsForUser/{coachId}")]
        public async Task<ActionResult<ResponseContract<List<TeamSummaryDto>>>> GetTeamsForUser(Guid coachId)
            => Ok(await teamService.GetTeamsForUserAsync(coachId, HttpContext.RequestAborted));

        /// <summary>Obsoleto: usar GET /api/v1/teams/{teamId}/members?roleId=</summary>
        [HttpPost("GetUsersForTeam")]
        public async Task<ActionResult<ResponseContract<List<TeamMemberDto>>>> GetUsersForTeam(RequestGetUserForTeamDto request)
            => Ok(await teamService.GetTeamMembersAsync(request.TeamId, request.RolId, HttpContext.RequestAborted));

        /// <summary>Obsoleto: usar PUT /api/v1/teams/{teamId}. Devuelve un booleano plano (contrato histórico).</summary>
        [HttpPost("UpdateTeam")]
        public async Task<ActionResult<bool>> UpdateTeam(RequestUpdateTeamDto requestUpdateTeamDto)
        {
            try
            {
                var result = await teamService.UpdateTeamAsync(requestUpdateTeamDto, HttpContext.RequestAborted);
                return Ok(result.Success);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // Contrato histórico: este endpoint devolvía false ante cualquier error.
                logger.LogError(ex, "Error al actualizar el equipo {TeamId}", requestUpdateTeamDto.TeamId);
                return Ok(false);
            }
        }

        /// <summary>Obsoleto: usar GET /api/v1/teams/{teamId}/strength-statistics/recent?coachId=</summary>
        [HttpPost("GetRecentStatistics")]
        public async Task<ActionResult<ResponseContract<List<StrengthTestSummaryDto>>>> GetRecentStatistics(RequestInfoCoachAndTeam request)
            => Ok(await teamService.GetRecentStatisticsAsync(request.CoachId, request.TeamId));
    }
}
