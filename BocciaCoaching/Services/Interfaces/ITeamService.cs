using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Models.DTO.Team;

namespace BocciaCoaching.Services.Interfaces
{
    public interface ITeamService
    {
        /// <summary>ES: Crea un equipo y vincula al entrenador como miembro.</summary>
        Task<ResponseContract<Guid>> CreateTeamAsync(RequestTeamDto requestTeamDto, CancellationToken cancellationToken = default);

        /// <summary>ES: Obtiene el resumen de un equipo.</summary>
        Task<ResponseContract<TeamSummaryDto>> GetTeamByIdAsync(Guid teamId, CancellationToken cancellationToken = default);

        /// <summary>ES: Equipos en los que el usuario es entrenador o miembro activo.</summary>
        Task<ResponseContract<List<TeamSummaryDto>>> GetTeamsForUserAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>ES: Integrantes del equipo, opcionalmente filtrados por rol.</summary>
        Task<ResponseContract<List<TeamMemberDto>>> GetTeamMembersAsync(Guid teamId, Guid? roleId, CancellationToken cancellationToken = default);

        /// <summary>ES: Agrega un usuario a un equipo.</summary>
        Task<ResponseContract<bool>> AddTeamMemberAsync(Guid teamId, Guid userId, CancellationToken cancellationToken = default);

        /// <summary>ES: Actualiza los datos editables del equipo.</summary>
        Task<ResponseContract<bool>> UpdateTeamAsync(RequestUpdateTeamDto requestUpdateTeamDto, CancellationToken cancellationToken = default);

        /// <summary>ES: Últimas estadísticas de fuerza del equipo para un entrenador.</summary>
        Task<ResponseContract<List<StrengthTestSummaryDto>>> GetRecentStatisticsAsync(Guid coachId, Guid teamId);
    }
}
