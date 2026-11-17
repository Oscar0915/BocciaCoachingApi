using System.Linq.Expressions;
using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.Teams
{
    public class TeamRepository(ApplicationDbContext context) : ITeamRepository
    {
        /// <summary>
        /// ES: Proyección a <see cref="TeamSummaryDto"/> traducida a SQL (el conteo de integrantes
        /// se resuelve con una subconsulta, sin cargar entidades en memoria).
        /// </summary>
        private Expression<Func<Team, TeamSummaryDto>> ToSummary => t => new TeamSummaryDto
        {
            TeamId = t.TeamId,
            NameTeam = t.NameTeam,
            Description = t.Description,
            CoachId = t.CoachId,
            Status = t.Status,
            Image = t.Image,
            Bc1 = t.Bc1,
            Bc2 = t.Bc2,
            Bc3 = t.Bc3,
            Bc4 = t.Bc4,
            Pairs = t.Pairs,
            Teams = t.Teams,
            Country = t.Country,
            Region = t.Region,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            MemberCount = context.TeamsUsers.Count(tu => tu.TeamId == t.TeamId && tu.Status)
        };

        public async Task<Guid> AddTeamAsync(RequestTeamDto requestTeamDto, CancellationToken cancellationToken = default)
        {
            var team = new Team
            {
                CoachId = requestTeamDto.CoachId,
                NameTeam = requestTeamDto.NameTeam,
                Description = requestTeamDto.Description,
                Image = requestTeamDto.Image,
                Bc1 = requestTeamDto.Bc1,
                Bc2 = requestTeamDto.Bc2,
                Bc3 = requestTeamDto.Bc3,
                Bc4 = requestTeamDto.Bc4,
                Pairs = requestTeamDto.Pairs,
                Teams = requestTeamDto.Teams,
                Country = requestTeamDto.Country,
                Region = requestTeamDto.Region,
                CreatedAt = DateTime.UtcNow
            };

            // El entrenador queda vinculado al equipo. Un único SaveChanges = una sola transacción:
            // antes eran dos escrituras y un fallo en la segunda dejaba el equipo sin entrenador.
            context.Teams.Add(team);
            context.TeamsUsers.Add(new TeamUser
            {
                Team = team,
                UserId = requestTeamDto.CoachId,
                DateCreation = DateTime.Now
            });

            await context.SaveChangesAsync(cancellationToken);
            return team.TeamId;
        }

        public Task<TeamSummaryDto?> GetTeamByIdAsync(Guid teamId, CancellationToken cancellationToken = default)
        {
            return context.Teams
                .AsNoTracking()
                .Where(t => t.TeamId == teamId)
                .Select(ToSummary)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<List<TeamSummaryDto>> GetTeamsForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Una sola consulta (antes: una para los IDs vía TeamUser + otra con Contains).
            return context.Teams
                .AsNoTracking()
                .Where(t => (t.Status == null || t.Status == true) &&
                            (t.CoachId == userId ||
                             context.TeamsUsers.Any(tu => tu.TeamId == t.TeamId && tu.UserId == userId && tu.Status)))
                .Select(ToSummary)
                .ToListAsync(cancellationToken);
        }

        public Task<List<TeamMemberDto>> GetTeamMembersAsync(Guid teamId, Guid? roleId, CancellationToken cancellationToken = default)
        {
            // Filtro por rol y proyección en SQL (antes se cargaban todos los usuarios con sus roles
            // en memoria y se filtraba después).
            var query = context.TeamsUsers
                .AsNoTracking()
                .Where(tu => tu.TeamId == teamId && tu.User != null);

            if (roleId.HasValue)
            {
                var role = roleId.Value;
                query = query.Where(tu => tu.User!.UserRoles!.Any(ur => ur.RolId == role));
            }

            return query
                .Select(tu => new TeamMemberDto
                {
                    UserId = tu.User!.UserId,
                    Dni = tu.User.Dni,
                    FirstName = tu.User.FirstName,
                    LastName = tu.User.LastName,
                    Email = tu.User.Email,
                    Address = tu.User.Address,
                    Country = tu.User.Country,
                    Image = tu.User.Image,
                    Category = tu.User.Category,
                    Seniority = tu.User.Seniority,
                    Status = tu.User.Status,
                    CreatedAt = tu.User.CreatedAt,
                    UpdatedAt = tu.User.UpdatedAt
                })
                .ToListAsync(cancellationToken);
        }

        public async Task AddTeamMemberAsync(Guid teamId, Guid userId, CancellationToken cancellationToken = default)
        {
            context.TeamsUsers.Add(new TeamUser
            {
                TeamId = teamId,
                UserId = userId,
                DateCreation = DateTime.Now
            });

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> UpdateTeamAsync(RequestUpdateTeamDto dto, CancellationToken cancellationToken = default)
        {
            var team = await context.Teams.FirstOrDefaultAsync(t => t.TeamId == dto.TeamId, cancellationToken);
            if (team is null)
                return false;

            if (!string.IsNullOrWhiteSpace(dto.Image))
                team.Image = dto.Image;

            team.Bc1 = dto.Bc1;
            team.Bc2 = dto.Bc2;
            team.Bc3 = dto.Bc3;
            team.Bc4 = dto.Bc4;
            team.Country = dto.Country;
            team.Region = dto.Region;
            team.UpdatedAt = DateTime.UtcNow;

            // La entidad ya está rastreada: EF solo actualiza las columnas que cambiaron
            // (context.Teams.Update marcaba todas las columnas como modificadas).
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<bool> TeamExistsAsync(Guid teamId, CancellationToken cancellationToken = default)
            => context.Teams.AnyAsync(t => t.TeamId == teamId, cancellationToken);

        public Task<bool> UserExistsAsync(Guid userId, CancellationToken cancellationToken = default)
            => context.Users.AnyAsync(u => u.UserId == userId, cancellationToken);

        public Task<bool> IsUserInTeam(Guid userId, Guid teamId, CancellationToken cancellationToken = default)
            => context.TeamsUsers.AnyAsync(tu => tu.UserId == userId && tu.TeamId == teamId, cancellationToken);

        public Task<int> CountTeamsByUserIdAsync(Guid userId)
            => context.Teams.CountAsync(t => t.CoachId == userId && (t.Status == null || t.Status == true));

        public Task<int> CountAthletesByTeamIdAsync(Guid teamId)
            => context.TeamsUsers.CountAsync(tu => tu.TeamId == teamId);
    }
}
