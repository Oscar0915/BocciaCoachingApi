using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using BocciaCoaching.Repositories.Statistic.Interfce;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IStatisticAssessStrength _statisticAssessStrength;
        public TeamService(ITeamRepository teamRepository, IStatisticAssessStrength statisticAssessStrength)
        {
            _teamRepository = teamRepository;
            _statisticAssessStrength = statisticAssessStrength;
        }

        /// <summary>
        /// Método para agregar un nuevo equipo
        /// </summary>
        /// <param name="requestTeamDto">Información del equipo</param>
        /// <returns></returns>
        public async Task<ResponseNewRecordDto> AddTeam(RequestTeamDto requestTeamDto)
        {
            return await _teamRepository.AddTeam(requestTeamDto);
        }

        /// <summary>
        /// Método para agregar mienbros al equipo
        /// </summary>
        /// <param name="requestTeamDto">Información del mienbro equipo</param>
        /// <returns></returns>
        public async Task<ResponseNewRecordDto> AddTeamMember(RequestTeamMemberDto requestTeamMemberDto)
        {
            return await _teamRepository.AddTeamMember(requestTeamMemberDto);
        }


        public async Task<List<Team>> GetTeamsForUser(RequestTeamDto requestTeamDto)
        {
            return  await _teamRepository.GetTeamsForUser(requestTeamDto);
        }

        public async Task<ResponseContract<List<User>>> GetUsersForTeam(RequestGetUserForTeamDto requestGetUserForTeamDto)
        {
            return await _teamRepository.GetUsersForTeam(requestGetUserForTeamDto);
        }

        public async Task<bool> UpdateTeam(RequestUpdateTeamDto requestUpdateImageTeamDto)
        {
            return await _teamRepository.UpdateTeam(requestUpdateImageTeamDto);
        }

        public async Task<ResponseContract<List<StrengthTestSummaryDto>>> GetRecentStatistics(RequestInfoCoachAndTeam requestInfoCoachAndTeam)
        {
            try
            {
                 var responseRecentSta = await _statisticAssessStrength.GetRecentStatistics(requestInfoCoachAndTeam.CoachId, requestInfoCoachAndTeam.TeamId);
                 return responseRecentSta;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
