using BocciaCoaching.Infrastructure.Api;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers.V1
{
    /// <summary>
    /// ES: Equipos (API REST v1).
    /// </summary>
    [Route("api/v1/teams")]
    [Tags("Teams")]
    public sealed class TeamsController(ITeamService teamService) : ApiControllerBase
    {
        private const string GetTeamRoute = "GetTeamById";
        private const string GetMembersRoute = "GetTeamMembers";

        /// <summary>Obtiene un equipo por su identificador.</summary>
        [HttpGet("{teamId:guid}", Name = GetTeamRoute)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<TeamSummaryDto>>> GetById(Guid teamId, CancellationToken cancellationToken)
            => FromResult(await teamService.GetTeamByIdAsync(teamId, cancellationToken));

        /// <summary>Crea un equipo. El entrenador queda vinculado automáticamente como miembro.</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<Guid>>> Create(RequestTeamDto request, CancellationToken cancellationToken)
        {
            var result = await teamService.CreateTeamAsync(request, cancellationToken);
            return CreatedFromResult(result, GetTeamRoute, new { teamId = result.Data });
        }

        /// <summary>Actualiza los datos editables de un equipo.</summary>
        [HttpPut("{teamId:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContract<bool>>> Update(Guid teamId, RequestUpdateTeamDto request, CancellationToken cancellationToken)
        {
            request.TeamId = teamId; // la ruta es la fuente de verdad del identificador
            return FromResult(await teamService.UpdateTeamAsync(request, cancellationToken));
        }

        /// <summary>Lista los integrantes de un equipo. Filtra por rol si se envía <c>roleId</c>.</summary>
        [HttpGet("{teamId:guid}/members", Name = GetMembersRoute)]
        public async Task<ActionResult<ResponseContract<List<TeamMemberDto>>>> GetMembers(
            Guid teamId, [FromQuery] Guid? roleId, CancellationToken cancellationToken)
            => FromResult(await teamService.GetTeamMembersAsync(teamId, roleId, cancellationToken));

        /// <summary>Agrega un usuario al equipo.</summary>
        [HttpPost("{teamId:guid}/members")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ResponseContract<bool>>> AddMember(
            Guid teamId, AddTeamMemberRequestDto request, CancellationToken cancellationToken)
        {
            var result = await teamService.AddTeamMemberAsync(teamId, request.UserId, cancellationToken);
            return CreatedFromResult(result, GetMembersRoute, new { teamId });
        }

        /// <summary>Últimas estadísticas de fuerza del equipo registradas por un entrenador.</summary>
        [HttpGet("{teamId:guid}/strength-statistics/recent")]
        public async Task<ActionResult<ResponseContract<List<StrengthTestSummaryDto>>>> GetRecentStatistics(
            Guid teamId, [FromQuery] Guid coachId)
            => FromResult(await teamService.GetRecentStatisticsAsync(coachId, teamId));

        /// <summary>Equipos en los que el usuario es entrenador o miembro activo.</summary>
        [HttpGet("~/api/v1/users/{userId:guid}/teams")]
        public async Task<ActionResult<ResponseContract<List<TeamSummaryDto>>>> GetTeamsForUser(Guid userId, CancellationToken cancellationToken)
            => FromResult(await teamService.GetTeamsForUserAsync(userId, cancellationToken));
    }
}
