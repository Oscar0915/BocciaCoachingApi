﻿﻿using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Models.DTO.Team;

namespace BocciaCoaching.Services.Interfaces
{
    public interface ITeamService
    {
        Task<ResponseContract<int>> AddTeam(RequestTeamDto requestTeamDto);
        /// <summary>
        /// Método para agregar mienbros al equipo
        /// </summary>
        /// <param name="requestTeamMemberDto">Información del mienbro equipo</param>
        /// <returns></returns>
        Task<ResponseContract<bool>> AddTeamMember(RequestTeamMemberDto requestTeamMemberDto);

        Task<ResponseContract<List<TeamSummaryDto>>> GetTeamsForUser(int coachId);
        Task<ResponseContract<List<TeamMemberDto>>> GetUsersForTeam(RequestGetUserForTeamDto requestGetUserForTeamDto);
        Task<bool> UpdateTeam(RequestUpdateTeamDto requestUpdateImageTeamDto);

        Task<ResponseContract<List<StrengthTestSummaryDto>>> GetRecentStatistics(
            RequestInfoCoachAndTeam requestInfoCoachAndTeam);
    }
}
