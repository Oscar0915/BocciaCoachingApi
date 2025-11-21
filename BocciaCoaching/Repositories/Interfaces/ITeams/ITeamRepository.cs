using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.ITeams
{
    public interface ITeamRepository
    {
        /// <summary>
        /// Método para agregar un nuevo equipo
        /// </summary>
        /// <param name="requestTeamDto">Información del equipo</param>
        /// <returns></returns>
        Task<ResponseNewRecordDto> AddTeam(RequestTeamDto requestTeamDto);


        /// <summary>
        /// Método para agregar mienbros al equipo
        /// </summary>
        /// <param name="requestTeamMemberDto"></param>
        /// <returns></returns>
        Task<ResponseNewRecordDto> AddTeamMember(RequestTeamMemberDto requestTeamMemberDto);

        /// <summary>
        /// Método para traer el listado de equipos relacionados a un usuario
        /// </summary>
        /// <param name="requestTeamDto"></param>
        /// <returns></returns>
        Task<List<Team>> GetTeamsForUser(RequestTeamDto requestTeamDto);

        Task<ResponseContract<List<User>>> GetUsersForTeam(RequestGetUserForTeamDto requestGetUserForTeamDto);
        Task<bool> UpdateTeam(RequestUpdateTeamDto requestUpdateImageTeamDto);
    }
}
