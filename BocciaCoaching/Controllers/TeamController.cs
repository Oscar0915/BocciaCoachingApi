using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {

        private readonly ITeamService _team;

        public TeamController(ITeamService teamService)
        {
            _team = teamService;
        }

        [HttpPost("AddNewTeam")]
        public async Task<ActionResult<IEnumerable<bool>>> Add(RequestTeamDto requestTeamDto )
        {
            var users = await _team.AddTeam(requestTeamDto);
            return Ok(users);
        }

        [HttpPost("AddNewTeamMember")]
        public async Task<ActionResult<IEnumerable<bool>>> AddMember(RequestTeamMemberDto requestTeamMemberDto)
        {
            var users = await _team.AddTeamMember(requestTeamMemberDto);
            return Ok(users);
        }
        [HttpPost("GetTeamsForUser")]
        public async Task<ActionResult<List<Team>>> GetTeamsForUser(RequestTeamDto requestTeamDto)
        {
            List<Team> teams = await _team.GetTeamsForUser(requestTeamDto);
            return Ok(teams);
        }


        [HttpPost("GetUsersForTeam")]
        public async Task<ActionResult<ResponseContract<List<User>>>> GetUsersForTeam(RequestGetUserForTeamDto requestGetUserForTeamDto)
        {
            var  teams = await _team.GetUsersForTeam(requestGetUserForTeamDto);
            return Ok(teams);
        }

        [HttpPost("UpdateTeam")]
        public async Task<ActionResult<bool>> UpdateTeam(RequestUpdateTeamDto requestUpdateImageTeamDto)
        {
            var responseImage = await _team.UpdateTeam(requestUpdateImageTeamDto);
            return Ok(responseImage);
        }
       
        [HttpPost("GetRecentStatistics")]
        public async Task<ActionResult<ResponseContract<List<StrengthTestSummaryDto>>>> GetRecentStatistics(RequestInfoCoachAndTeam requestInfoCoachAndTeam)
        {
            var responseImage = await _team.GetRecentStatistics(requestInfoCoachAndTeam);
            return Ok(responseImage);
        }
    }
}
