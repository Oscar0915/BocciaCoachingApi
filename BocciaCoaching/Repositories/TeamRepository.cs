using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        /// <summary>
        /// Método para obtener los equipos al que pertenece los entrenadores 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Team>> GetTeamsForUser(RequestTeamDto requestTeamDto)
        {
            try
            {
                var teams = await _context.TeamsUsers
                    .Where(tu => tu.UserId == requestTeamDto.CoachId)
                    .Include(tu => tu.Team) // Incluye la info del equipo
                    .Select(tu => tu.Team)  // Proyecta solo el Team
                    .ToListAsync();

                return teams;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetTeamsForUserAsync: {ex.Message}");
                return new List<Team>(); // Retorna lista vacía si hay error
            }
        }
        /// <summary>
        /// Método para obtener los integrantes de un equipo
        /// </summary>
        /// <param name="requestGetUserForTeamDto"></param>
        /// <returns></returns>
        public async Task<List<User>> GetUsersForTeam(RequestGetUserForTeamDto requestGetUserForTeamDto)
        {
            try
            {
                var users = await _context.TeamsUsers
                .Where(tu => tu.TeamId == requestGetUserForTeamDto.TeamId &&
                             tu.User.UserRoles.Any(ru => ru.RolId == requestGetUserForTeamDto.RolId))
                .Include(tu => tu.User)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(ru => ru.Rol)
                .Select(tu => tu.User)
                .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetUsersForTeam: {ex.Message}");
                return new List<User>();
            }
        }


        /// <summary>
        /// Método para actualizar la imagen del equipo
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="base64Image"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> UpdateTeamImageAsync(RequestUpdateImageTeamDto requestUpdateImageTeamDto)
        {
            try
            {
                var team = await _context.Teams.FirstOrDefaultAsync(t => t.TeamId == requestUpdateImageTeamDto.TeamId);

                if (team == null)
                    return false;

                // Validar que la cadena base64 sea válida
                if (string.IsNullOrWhiteSpace(requestUpdateImageTeamDto.Image))
                    throw new ArgumentException("La imagen no puede estar vacía.");

                // Si quieres validar que realmente sea base64
                if (!IsBase64String(requestUpdateImageTeamDto.Image))
                    throw new ArgumentException("El formato de imagen no es válido.");

                team.Image = requestUpdateImageTeamDto.Image;
                team.UpdatedAt = DateTime.UtcNow;

                _context.Teams.Update(team);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error si tienes un sistema de auditoría
                Console.WriteLine($"Error al actualizar la imagen: {ex.Message}");
                return false;
            }
        }

        private bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }



    }
}
