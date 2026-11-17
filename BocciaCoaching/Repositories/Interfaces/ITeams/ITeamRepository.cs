using BocciaCoaching.Models.DTO.Team;

namespace BocciaCoaching.Repositories.Interfaces.ITeams
{
    /// <summary>
    /// ES: Acceso a datos de equipos. Devuelve datos (no ResponseContract): las reglas de negocio
    /// y los mensajes viven en <c>TeamService</c>.
    /// </summary>
    public interface ITeamRepository
    {
        /// <summary>
        /// ES: Crea el equipo y vincula al entrenador como miembro en una sola transacción.
        /// </summary>
        /// <returns>Identificador del equipo creado.</returns>
        Task<Guid> AddTeamAsync(RequestTeamDto requestTeamDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// ES: Obtiene el resumen de un equipo (incluye cantidad de integrantes activos).
        /// </summary>
        Task<TeamSummaryDto?> GetTeamByIdAsync(Guid teamId, CancellationToken cancellationToken = default);

        /// <summary>
        /// ES: Equipos activos en los que el usuario es entrenador o miembro activo.
        /// </summary>
        Task<List<TeamSummaryDto>> GetTeamsForUserAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// ES: Integrantes del equipo, opcionalmente filtrados por rol (sin datos sensibles).
        /// </summary>
        Task<List<TeamMemberDto>> GetTeamMembersAsync(Guid teamId, Guid? roleId, CancellationToken cancellationToken = default);

        /// <summary>
        /// ES: Agrega un usuario al equipo.
        /// </summary>
        Task AddTeamMemberAsync(Guid teamId, Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// ES: Actualiza los datos editables del equipo.
        /// </summary>
        /// <returns><c>false</c> si el equipo no existe.</returns>
        Task<bool> UpdateTeamAsync(RequestUpdateTeamDto requestUpdateTeamDto, CancellationToken cancellationToken = default);

        Task<bool> TeamExistsAsync(Guid teamId, CancellationToken cancellationToken = default);

        Task<bool> UserExistsAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Verificar si un usuario ya pertenece a un equipo
        /// </summary>
        Task<bool> IsUserInTeam(Guid userId, Guid teamId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Contar la cantidad de equipos que ha creado un usuario
        /// </summary>
        Task<int> CountTeamsByUserIdAsync(Guid userId);

        /// <summary>
        /// Contar la cantidad de atletas en un equipo
        /// </summary>
        Task<int> CountAthletesByTeamIdAsync(Guid teamId);
    }
}
