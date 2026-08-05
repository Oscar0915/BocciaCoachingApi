using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IWellness;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.Wellness
{
    public class WellnessRepository : IWellnessRepository
    {
        private readonly ApplicationDbContext _context;

        public WellnessRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseContract<ResponseDailyWellnessDto>> CreateIfNotExistsForDayAsync(AddDailyWellnessDto dto)
        {
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                var assessmentDate = (dto.AssessmentDate ?? DateTime.Now).Date;

                var alreadyExists = await _context.DailyWellnessAssessments
                    .AnyAsync(w => w.AthleteId == dto.AthleteId && w.AssessmentDate.Date == assessmentDate);

                if (alreadyExists)
                {
                    await transaction.RollbackAsync();
                    return ResponseContract<ResponseDailyWellnessDto>.Fail(
                        "El atleta ya registró su test de bienestar para este día.");
                }

                var totalScore = dto.Sleep + dto.Stress + dto.Fatigue + dto.MusclePain;
                var averageScore = Math.Round(totalScore / 4.0, 2);

                var entity = new DailyWellness
                {
                    AthleteId = dto.AthleteId,
                    TeamId = dto.TeamId,
                    AssessmentDate = assessmentDate,
                    Sleep = dto.Sleep,
                    Stress = dto.Stress,
                    Fatigue = dto.Fatigue,
                    MusclePain = dto.MusclePain,
                    TotalScore = totalScore,
                    AverageScore = averageScore,
                    Observations = dto.Observations,
                    CreatedAt = DateTime.Now
                };

                await _context.DailyWellnessAssessments.AddAsync(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var athlete = await _context.Users.FindAsync(entity.AthleteId);

                return ResponseContract<ResponseDailyWellnessDto>.Ok(
                    MapToDto(entity, athlete),
                    "Test de bienestar registrado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CreateIfNotExistsForDayAsync: {ex.Message}");
                return ResponseContract<ResponseDailyWellnessDto>.Fail($"Error al registrar el test de bienestar: {ex.Message}");
            }
        }

        public async Task<ResponseContract<ResponseDailyWellnessDto>> UpdateAsync(UpdateDailyWellnessDto dto)
        {
            try
            {
                var entity = await _context.DailyWellnessAssessments.FindAsync(dto.DailyWellnessId);
                if (entity == null)
                    return ResponseContract<ResponseDailyWellnessDto>.Fail("No se encontró el test de bienestar indicado.");

                entity.Sleep = dto.Sleep;
                entity.Stress = dto.Stress;
                entity.Fatigue = dto.Fatigue;
                entity.MusclePain = dto.MusclePain;
                entity.Observations = dto.Observations;
                entity.TotalScore = dto.Sleep + dto.Stress + dto.Fatigue + dto.MusclePain;
                entity.AverageScore = Math.Round(entity.TotalScore / 4.0, 2);
                entity.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                var athlete = await _context.Users.FindAsync(entity.AthleteId);

                return ResponseContract<ResponseDailyWellnessDto>.Ok(
                    MapToDto(entity, athlete),
                    "Test de bienestar actualizado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateAsync: {ex.Message}");
                return ResponseContract<ResponseDailyWellnessDto>.Fail($"Error al actualizar el test de bienestar: {ex.Message}");
            }
        }

        public async Task<ResponseDailyWellnessDto?> GetByAthleteAndDateAsync(Guid athleteId, DateTime date)
        {
            var targetDate = date.Date;
            var entity = await _context.DailyWellnessAssessments
                .Include(w => w.Athlete)
                .FirstOrDefaultAsync(w => w.AthleteId == athleteId && w.AssessmentDate.Date == targetDate);

            return entity == null ? null : MapToDto(entity, entity.Athlete);
        }

        public async Task<ResponseDailyWellnessDto?> GetByIdAsync(Guid dailyWellnessId)
        {
            var entity = await _context.DailyWellnessAssessments
                .Include(w => w.Athlete)
                .FirstOrDefaultAsync(w => w.DailyWellnessId == dailyWellnessId);

            return entity == null ? null : MapToDto(entity, entity.Athlete);
        }

        public async Task<WellnessAthleteHistoryDto?> GetAthleteHistoryAsync(Guid athleteId)
        {
            var athlete = await _context.Users.FindAsync(athleteId);
            if (athlete == null)
                return null;

            var records = await _context.DailyWellnessAssessments
                .Where(w => w.AthleteId == athleteId)
                .OrderByDescending(w => w.AssessmentDate)
                .ToListAsync();

            return new WellnessAthleteHistoryDto
            {
                AthleteId = athleteId,
                AthleteName = $"{athlete.FirstName} {athlete.LastName}".Trim(),
                TotalRecords = records.Count,
                OverallAverageScore = records.Count > 0 ? Math.Round(records.Average(r => r.AverageScore), 2) : null,
                Records = records.Select(r => MapToDto(r, athlete)).ToList()
            };
        }

        public async Task<List<ResponseDailyWellnessDto>> GetTeamWellnessByDateAsync(Guid teamId, DateTime date)
        {
            var targetDate = date.Date;
            var records = await _context.DailyWellnessAssessments
                .Include(w => w.Athlete)
                .Where(w => w.TeamId == teamId && w.AssessmentDate.Date == targetDate)
                .OrderBy(w => w.AthleteId)
                .ToListAsync();

            return records.Select(r => MapToDto(r, r.Athlete)).ToList();
        }

        private static ResponseDailyWellnessDto MapToDto(DailyWellness entity, User? athlete)
        {
            return new ResponseDailyWellnessDto
            {
                DailyWellnessId = entity.DailyWellnessId,
                AthleteId = entity.AthleteId,
                AthleteName = athlete != null ? $"{athlete.FirstName} {athlete.LastName}".Trim() : null,
                TeamId = entity.TeamId,
                AssessmentDate = entity.AssessmentDate,
                Sleep = entity.Sleep,
                Stress = entity.Stress,
                Fatigue = entity.Fatigue,
                MusclePain = entity.MusclePain,
                TotalScore = entity.TotalScore,
                AverageScore = entity.AverageScore,
                Observations = entity.Observations
            };
        }
    }
}

