using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Services.Interfaces
{
    public interface ITeamService
    {
        Task<ResponseNewRecordDto> AddTeam(RequestTeamDto requestTeamDto);
        /// <summary>
        /// Método para agregar mienbros al equipo
        /// </summary>
        /// <param name="requestTeamDto">Información del mienbro equipo</param>
        /// <returns></returns>
        Task<ResponseNewRecordDto> AddTeamMember(RequestTeamMemberDto requestTeamMemberDto);

        Task<List<Team>> GetTeamsForUser(RequestTeamDto requestTeamDto);
        Task<List<User>> GetUsersForTeam(RequestGetUserForTeamDto requestGetUserForTeamDto);
        Task<bool> UpdateTeamImageAsync(RequestUpdateImageTeamDto requestUpdateImageTeamDto);
    }
}
