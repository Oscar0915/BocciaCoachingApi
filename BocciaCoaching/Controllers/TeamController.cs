﻿﻿using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Services.Interfaces;
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
        public async Task<ActionResult<ResponseContract<int>>> Add(RequestTeamDto requestTeamDto )
        {
            var response = await _team.AddTeam(requestTeamDto);
            return Ok(response);
        }

        [HttpPost("AddNewTeamMember")]
        public async Task<ActionResult<ResponseContract<bool>>> AddMember(RequestTeamMemberDto requestTeamMemberDto)
        {
            var response = await _team.AddTeamMember(requestTeamMemberDto);
            return Ok(response);
        }
        [HttpGet("GetTeamsForUser/{coachId}")]
        public async Task<ActionResult<ResponseContract<List<TeamSummaryDto>>>> GetTeamsForUser(int coachId)
        {
            var response = await _team.GetTeamsForUser(coachId);
            return Ok(response);
        }


        [HttpPost("GetUsersForTeam")]
        public async Task<ActionResult<ResponseContract<List<TeamMemberDto>>>> GetUsersForTeam(RequestGetUserForTeamDto requestGetUserForTeamDto)
        {
            var teams = await _team.GetUsersForTeam(requestGetUserForTeamDto);
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
