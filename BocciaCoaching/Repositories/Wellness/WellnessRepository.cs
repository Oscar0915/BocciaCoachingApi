using System.Linq.Expressions;
using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IWellness;
using BocciaCoaching.Utils;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.Wellness
{
    public class WellnessRepository(ApplicationDbContext context) : IWellnessRepository
    {
        /// <summary>
        /// ES: Proyección traducida a SQL: solo trae las columnas necesarias (incluido el nombre del
        /// atleta), sin cargar ni rastrear la entidad User completa.
        /// </summary>
        private static readonly Expression<Func<DailyWellness, ResponseDailyWellnessDto>> ToDto = w => new ResponseDailyWellnessDto
        {
            DailyWellnessId = w.DailyWellnessId,
            AthleteId = w.AthleteId,
            AthleteName = w.Athlete != null ? ((w.Athlete.FirstName ?? "") + " " + (w.Athlete.LastName ?? "")).Trim() : null,
            TeamId = w.TeamId,
            AssessmentDate = w.AssessmentDate,
            ScaleVersion = w.ScaleVersion,
            Fatigue = w.Fatigue,
            Sleep = w.Sleep,
            MusclePain = w.MusclePain,
            Stress = w.Stress,
            Mood = w.Mood,
            TotalScore = w.TotalScore,
            AverageScore = w.AverageScore,
            ReadinessLevel = w.ReadinessLevel,
            VetoApplied = w.VetoApplied,
            Observations = w.Observations,
            CreatedAt = w.CreatedAt,
            UpdatedAt = w.UpdatedAt
        };

        /// <summary>
        /// ES: Filtro por día como rango [inicio, inicio+1). A diferencia de <c>AssessmentDate.Date == x</c>,
        /// permite usar el índice (AthleteId, AssessmentDate).
        /// </summary>
        private static Expression<Func<DailyWellness, bool>> OnDay(DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);
            return w => w.AssessmentDate >= start && w.AssessmentDate < end;
        }

        /// <summary>ES: Miembros activos del equipo con rol Atleta.</summary>
        private IQueryable<TeamUser> TeamAthletes(Guid teamId)
            => context.TeamsUsers.Where(tu => tu.TeamId == teamId && tu.Status && tu.User != null &&
                                              tu.User.UserRoles!.Any(ur => ur.RolId == RoleIds.Athlete));

        public Task<bool> ExistsForDayAsync(Guid athleteId, DateTime day, CancellationToken cancellationToken = default)
            => context.DailyWellnessAssessments
                .Where(w => w.AthleteId == athleteId)
                .Where(OnDay(day))
                .AnyAsync(cancellationToken);

        public async Task AddAsync(DailyWellness entity, CancellationToken cancellationToken = default)
        {
            context.DailyWellnessAssessments.Add(entity);
            await context.SaveChangesAsync(cancellationToken);
        }

        public Task<DailyWellness?> GetForUpdateAsync(Guid dailyWellnessId, CancellationToken cancellationToken = default)
            => context.DailyWellnessAssessments.FirstOrDefaultAsync(w => w.DailyWellnessId == dailyWellnessId, cancellationToken);

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => context.SaveChangesAsync(cancellationToken);

        public Task<bool> AthleteExistsAsync(Guid athleteId, CancellationToken cancellationToken = default)
            => context.Users.AnyAsync(u => u.UserId == athleteId, cancellationToken);

        public Task<bool> TeamExistsAsync(Guid teamId, CancellationToken cancellationToken = default)
            => context.Teams.AnyAsync(t => t.TeamId == teamId, cancellationToken);

        public Task<bool> IsCoachOfAthleteAsync(Guid coachId, Guid athleteId, CancellationToken cancellationToken = default)
            => context.TeamsUsers
                .Where(member => member.UserId == athleteId && member.Status)
                .AnyAsync(member =>
                    context.Teams.Any(t => t.TeamId == member.TeamId && t.CoachId == coachId) ||
                    context.TeamsUsers.Any(c => c.TeamId == member.TeamId && c.UserId == coachId && c.Status &&
                                                c.User!.UserRoles!.Any(ur => ur.RolId == RoleIds.Coach)),
                    cancellationToken);

        public Task<bool> IsCoachOfTeamAsync(Guid coachId, Guid teamId, CancellationToken cancellationToken = default)
            => context.Teams.AnyAsync(t => t.TeamId == teamId &&
                (t.CoachId == coachId ||
                 context.TeamsUsers.Any(c => c.TeamId == teamId && c.UserId == coachId && c.Status &&
                                             c.User!.UserRoles!.Any(ur => ur.RolId == RoleIds.Coach))),
                cancellationToken);

        public Task<ResponseDailyWellnessDto?> GetByAthleteAndDateAsync(Guid athleteId, DateTime date, CancellationToken cancellationToken = default)
            => context.DailyWellnessAssessments
                .AsNoTracking()
                .Where(w => w.AthleteId == athleteId)
                .Where(OnDay(date))
                .Select(ToDto)
                .FirstOrDefaultAsync(cancellationToken);

        public Task<ResponseDailyWellnessDto?> GetByIdAsync(Guid dailyWellnessId, CancellationToken cancellationToken = default)
            => context.DailyWellnessAssessments
                .AsNoTracking()
                .Where(w => w.DailyWellnessId == dailyWellnessId)
                .Select(ToDto)
                .FirstOrDefaultAsync(cancellationToken);

        public async Task<WellnessAthleteHistoryDto?> GetAthleteHistoryAsync(Guid athleteId, CancellationToken cancellationToken = default)
        {
            var athleteName = await context.Users
                .AsNoTracking()
                .Where(u => u.UserId == athleteId)
                .Select(u => ((u.FirstName ?? "") + " " + (u.LastName ?? "")).Trim())
                .FirstOrDefaultAsync(cancellationToken);

            if (athleteName is null)
                return null;

            var records = await context.DailyWellnessAssessments
                .AsNoTracking()
                .Where(w => w.AthleteId == athleteId)
                .OrderByDescending(w => w.AssessmentDate)
                .Select(ToDto)
                .ToListAsync(cancellationToken);

            var mcLean = records.Where(r => r.ScaleVersion == DailyWellness.McLeanScaleVersion).ToList();

            return new WellnessAthleteHistoryDto
            {
                AthleteId = athleteId,
                AthleteName = athleteName,
                TotalRecords = records.Count,
                AverageTotalScore = mcLean.Count > 0 ? Math.Round(mcLean.Average(r => r.TotalScore), 2) : null,
                OverallAverageScore = mcLean.Count > 0 ? Math.Round(mcLean.Average(r => r.AverageScore), 2) : null,
                GreenDays = mcLean.Count(r => r.ReadinessLevel == "Green"),
                YellowDays = mcLean.Count(r => r.ReadinessLevel == "Yellow"),
                RedDays = mcLean.Count(r => r.ReadinessLevel == "Red"),
                Records = records
            };
        }

        public Task<List<WellnessPendingAthleteDto>> GetTeamAthletesAsync(Guid teamId, CancellationToken cancellationToken = default)
            => TeamAthletes(teamId)
                .AsNoTracking()
                .Select(tu => new WellnessPendingAthleteDto
                {
                    AthleteId = tu.UserId,
                    AthleteName = ((tu.User!.FirstName ?? "") + " " + (tu.User.LastName ?? "")).Trim()
                })
                .Distinct()
                .ToListAsync(cancellationToken);

        public Task<List<ResponseDailyWellnessDto>> GetTeamWellnessByDateAsync(Guid teamId, DateTime date, CancellationToken cancellationToken = default)
        {
            var athleteIds = TeamAthletes(teamId).Select(tu => tu.UserId);

            return context.DailyWellnessAssessments
                .AsNoTracking()
                .Where(w => athleteIds.Contains(w.AthleteId) || w.TeamId == teamId)
                .Where(OnDay(date))
                .OrderBy(w => w.TotalScore)
                .Select(ToDto)
                .ToListAsync(cancellationToken);
        }
    }
}
