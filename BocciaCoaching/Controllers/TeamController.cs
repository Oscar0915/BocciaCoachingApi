using BocciaCoaching.Models.DTO.Team;
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
    }
}
