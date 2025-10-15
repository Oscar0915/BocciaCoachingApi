using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces;
using System.Threading.Tasks;

namespace BocciaCoaching.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Método para agregar un nuevo equipo
        /// </summary>
        /// <param name="requestTeamDto">Información del equipo</param>
        /// <returns></returns>
        public async Task<ResponseNewRecordDto> AddTeam(RequestTeamDto requestTeamDto)
        {
            try
            {
                var team = new Team()
                {
                    CoachId = requestTeamDto.CoachId,
                    NameTeam = requestTeamDto.NameTeam,
                    Description = requestTeamDto.Description,
                };

                await _context.Teams.AddAsync(team);
                await _context.SaveChangesAsync();
                return new ResponseNewRecordDto()
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseNewRecordDto()
                {
                    Success = true,
                    Message = ex.Message
                   
                };
            }

        }


        /// <summary>
        /// Método para agregar mienbros al equipo
        /// </summary>
        /// <param name="requestTeamDto">Información del mienbro equipo</param>
        /// <returns></returns>
        public async Task<ResponseNewRecordDto> AddTeamMember(RequestTeamMemberDto requestTeamMemberDto)
        {
            try
            {
                var teamUser = new TeamUser()
                {
                    TeamId = requestTeamMemberDto.TeamId,
                    UserId = requestTeamMemberDto.UserId,
                    DateCreation = new DateTime(),
                };

                await _context.TeamsUsers.AddAsync(teamUser);
                await _context.SaveChangesAsync();
                return new ResponseNewRecordDto()
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseNewRecordDto()
                {
                    Success = true,
                    Message = ex.Message

                };
            }

        }

    }
}
