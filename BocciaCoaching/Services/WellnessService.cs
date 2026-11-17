using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IWellness;
using BocciaCoaching.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace BocciaCoaching.Services
{
    /// <summary>
    /// ES: Reglas de negocio del test diario de bienestar (escala 1–7 en Sueño, Estrés, Fatiga y
    /// Dolor Muscular; un registro por atleta por día). Los errores inesperados se propagan al
    /// <c>GlobalExceptionHandler</c>.
    /// </summary>
    public class WellnessService(IWellnessRepository repository) : IWellnessService
    {
        private const int MinScore = 1;
        private const int MaxScore = 7;
        private const string OutOfRangeMessage = "Todos los indicadores deben estar entre 1 y 7.";
        private const string AlreadyRegisteredMessage = "El atleta ya registró su test de bienestar para este día.";
        private const string NotFoundMessage = "No se encontró el test de bienestar indicado.";

        public async Task<ResponseContract<ResponseDailyWellnessDto>> RegisterDailyWellnessAsync(AddDailyWellnessDto dto, CancellationToken cancellationToken = default)
        {
            if (!AreValuesInRange(dto.Sleep, dto.Stress, dto.Fatigue, dto.MusclePain))
                return ResponseContract<ResponseDailyWellnessDto>.Invalid(OutOfRangeMessage);

            var assessmentDate = (dto.AssessmentDate ?? DateTime.Now).Date;

            if (await repository.ExistsForDayAsync(dto.AthleteId, assessmentDate, cancellationToken))
                return ResponseContract<ResponseDailyWellnessDto>.Conflict(AlreadyRegisteredMessage);

            if (!await repository.AthleteExistsAsync(dto.AthleteId, cancellationToken))
                return ResponseContract<ResponseDailyWellnessDto>.NotFound("El atleta indicado no existe.");

            var (total, average) = CalculateScores(dto.Sleep, dto.Stress, dto.Fatigue, dto.MusclePain);
            var entity = new DailyWellness
            {
                AthleteId = dto.AthleteId,
                TeamId = dto.TeamId,
                AssessmentDate = assessmentDate,
                Sleep = dto.Sleep,
                Stress = dto.Stress,
                Fatigue = dto.Fatigue,
                MusclePain = dto.MusclePain,
                TotalScore = total,
                AverageScore = average,
                Observations = dto.Observations,
                CreatedAt = DateTime.Now
            };

            try
            {
                await repository.AddAsync(entity, cancellationToken);
            }
            catch (DbUpdateException ex) when (IsDuplicateKey(ex))
            {
                // Dos envíos simultáneos: el índice único (atleta, día) garantiza un solo registro.
                return ResponseContract<ResponseDailyWellnessDto>.Conflict(AlreadyRegisteredMessage);
            }

            var created = await repository.GetByIdAsync(entity.DailyWellnessId, cancellationToken);
            return ResponseContract<ResponseDailyWellnessDto>.Created(created!, "Test de bienestar registrado correctamente");
        }

        public async Task<ResponseContract<ResponseDailyWellnessDto>> UpdateDailyWellnessAsync(UpdateDailyWellnessDto dto, CancellationToken cancellationToken = default)
        {
            if (!AreValuesInRange(dto.Sleep, dto.Stress, dto.Fatigue, dto.MusclePain))
                return ResponseContract<ResponseDailyWellnessDto>.Invalid(OutOfRangeMessage);

            var entity = await repository.GetForUpdateAsync(dto.DailyWellnessId, cancellationToken);
            if (entity is null)
                return ResponseContract<ResponseDailyWellnessDto>.NotFound(NotFoundMessage);

            var (total, average) = CalculateScores(dto.Sleep, dto.Stress, dto.Fatigue, dto.MusclePain);
            entity.Sleep = dto.Sleep;
            entity.Stress = dto.Stress;
            entity.Fatigue = dto.Fatigue;
            entity.MusclePain = dto.MusclePain;
            entity.Observations = dto.Observations;
            entity.TotalScore = total;
            entity.AverageScore = average;
            entity.UpdatedAt = DateTime.Now;

            await repository.SaveChangesAsync(cancellationToken);

            var updated = await repository.GetByIdAsync(entity.DailyWellnessId, cancellationToken);
            return ResponseContract<ResponseDailyWellnessDto>.Ok(updated!, "Test de bienestar actualizado correctamente");
        }

        public Task<ResponseContract<ResponseDailyWellnessDto>> GetTodayWellnessAsync(Guid athleteId, CancellationToken cancellationToken = default)
            => GetWellnessByDateAsync(athleteId, DateTime.Now, cancellationToken);

        public async Task<ResponseContract<ResponseDailyWellnessDto>> GetWellnessByDateAsync(Guid athleteId, DateTime date, CancellationToken cancellationToken = default)
        {
            var result = await repository.GetByAthleteAndDateAsync(athleteId, date, cancellationToken);
            return result is null
                ? ResponseContract<ResponseDailyWellnessDto>.NotFound("El atleta no ha registrado su test de bienestar para esta fecha.")
                : ResponseContract<ResponseDailyWellnessDto>.Ok(result);
        }

        public async Task<ResponseContract<ResponseDailyWellnessDto>> GetWellnessByIdAsync(Guid dailyWellnessId, CancellationToken cancellationToken = default)
        {
            var result = await repository.GetByIdAsync(dailyWellnessId, cancellationToken);
            return result is null
                ? ResponseContract<ResponseDailyWellnessDto>.NotFound(NotFoundMessage)
                : ResponseContract<ResponseDailyWellnessDto>.Ok(result);
        }

        public async Task<ResponseContract<WellnessAthleteHistoryDto>> GetAthleteHistoryAsync(Guid athleteId, CancellationToken cancellationToken = default)
        {
            var result = await repository.GetAthleteHistoryAsync(athleteId, cancellationToken);
            return result is null
                ? ResponseContract<WellnessAthleteHistoryDto>.NotFound("No se encontró el atleta indicado.")
                : ResponseContract<WellnessAthleteHistoryDto>.Ok(result);
        }

        public async Task<ResponseContract<List<ResponseDailyWellnessDto>>> GetTeamWellnessByDateAsync(Guid teamId, DateTime date, CancellationToken cancellationToken = default)
        {
            var result = await repository.GetTeamWellnessByDateAsync(teamId, date, cancellationToken);
            return ResponseContract<List<ResponseDailyWellnessDto>>.Ok(result);
        }

        private static bool AreValuesInRange(params int[] values)
            => values.All(v => v is >= MinScore and <= MaxScore);

        private static (int Total, double Average) CalculateScores(int sleep, int stress, int fatigue, int musclePain)
        {
            var total = sleep + stress + fatigue + musclePain;
            return (total, Math.Round(total / 4.0, 2));
        }

        private static bool IsDuplicateKey(DbUpdateException ex)
            => ex.InnerException is MySqlException { ErrorCode: MySqlErrorCode.DuplicateKeyEntry };
    }
}
