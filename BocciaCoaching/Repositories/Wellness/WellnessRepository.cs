using System.Linq.Expressions;
using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IWellness;
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
            Sleep = w.Sleep,
            Stress = w.Stress,
            Fatigue = w.Fatigue,
            MusclePain = w.MusclePain,
            TotalScore = w.TotalScore,
            AverageScore = w.AverageScore,
            Observations = w.Observations
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

            return new WellnessAthleteHistoryDto
            {
                AthleteId = athleteId,
                AthleteName = athleteName,
                TotalRecords = records.Count,
                OverallAverageScore = records.Count > 0 ? Math.Round(records.Average(r => r.AverageScore), 2) : null,
                Records = records
            };
        }

        public Task<List<ResponseDailyWellnessDto>> GetTeamWellnessByDateAsync(Guid teamId, DateTime date, CancellationToken cancellationToken = default)
            => context.DailyWellnessAssessments
                .AsNoTracking()
                .Where(w => w.TeamId == teamId)
                .Where(OnDay(date))
                .OrderBy(w => w.AthleteId)
                .Select(ToDto)
                .ToListAsync(cancellationToken);
    }
}
