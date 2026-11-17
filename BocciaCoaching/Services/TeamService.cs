using System.Buffers.Text;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using BocciaCoaching.Repositories.Statistic.Interfce;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    /// <summary>
    /// ES: Reglas de negocio de equipos. Los errores esperados se devuelven como
    /// <see cref="ResponseContract{T}"/> con su <see cref="ResultStatus"/>; los inesperados se
    /// propagan y los maneja <c>GlobalExceptionHandler</c> (log + respuesta estándar).
    /// </summary>
    public class TeamService(ITeamRepository teamRepository, IStatisticAssessStrength statisticAssessStrength) : ITeamService
    {
        public async Task<ResponseContract<Guid>> CreateTeamAsync(RequestTeamDto requestTeamDto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(requestTeamDto.NameTeam))
                return ResponseContract<Guid>.Invalid("El nombre del equipo es obligatorio.");

            if (!await teamRepository.UserExistsAsync(requestTeamDto.CoachId, cancellationToken))
                return ResponseContract<Guid>.NotFound("El entrenador indicado no existe.");

            var teamId = await teamRepository.AddTeamAsync(requestTeamDto, cancellationToken);
            return ResponseContract<Guid>.Created(teamId, "Equipo creado exitosamente");
        }

        public async Task<ResponseContract<TeamSummaryDto>> GetTeamByIdAsync(Guid teamId, CancellationToken cancellationToken = default)
        {
            var team = await teamRepository.GetTeamByIdAsync(teamId, cancellationToken);
            return team is null
                ? ResponseContract<TeamSummaryDto>.NotFound("El equipo no existe.")
                : ResponseContract<TeamSummaryDto>.Ok(team, "Equipo obtenido satisfactoriamente");
        }

        public async Task<ResponseContract<List<TeamSummaryDto>>> GetTeamsForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var teams = await teamRepository.GetTeamsForUserAsync(userId, cancellationToken);
            return ResponseContract<List<TeamSummaryDto>>.Ok(teams, "Equipos obtenidos satisfactoriamente");
        }

        public async Task<ResponseContract<List<TeamMemberDto>>> GetTeamMembersAsync(Guid teamId, Guid? roleId, CancellationToken cancellationToken = default)
        {
            var members = await teamRepository.GetTeamMembersAsync(teamId, roleId, cancellationToken);
            return ResponseContract<List<TeamMemberDto>>.Ok(members, "Usuarios obtenidos satisfactoriamente");
        }

        public async Task<ResponseContract<bool>> AddTeamMemberAsync(Guid teamId, Guid userId, CancellationToken cancellationToken = default)
        {
            if (!await teamRepository.TeamExistsAsync(teamId, cancellationToken))
                return ResponseContract<bool>.NotFound("El equipo no existe.");

            if (!await teamRepository.UserExistsAsync(userId, cancellationToken))
                return ResponseContract<bool>.NotFound("El usuario no existe.");

            if (await teamRepository.IsUserInTeam(userId, teamId, cancellationToken))
                return ResponseContract<bool>.Conflict("El atleta ya pertenece a este equipo");

            await teamRepository.AddTeamMemberAsync(teamId, userId, cancellationToken);
            return ResponseContract<bool>.Created(true, "Miembro agregado exitosamente al equipo");
        }

        public async Task<ResponseContract<bool>> UpdateTeamAsync(RequestUpdateTeamDto requestUpdateTeamDto, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(requestUpdateTeamDto.Image) && !IsBase64(requestUpdateTeamDto.Image))
                return ResponseContract<bool>.Invalid("El formato de imagen no es válido.");

            var updated = await teamRepository.UpdateTeamAsync(requestUpdateTeamDto, cancellationToken);
            return updated
                ? ResponseContract<bool>.Ok(true, "Equipo actualizado correctamente")
                : ResponseContract<bool>.NotFound("El equipo no existe.");
        }

        public Task<ResponseContract<List<StrengthTestSummaryDto>>> GetRecentStatisticsAsync(Guid coachId, Guid teamId)
            => statisticAssessStrength.GetRecentStatistics(coachId, teamId);

        /// <summary>
        /// ES: Valida base64 sin asignar un buffer del tamaño de la cadena (las imágenes pueden pesar MB).
        /// </summary>
        private static bool IsBase64(string value) => Base64.IsValid(value);
    }
}
