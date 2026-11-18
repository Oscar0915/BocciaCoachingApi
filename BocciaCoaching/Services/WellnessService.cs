using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IWellness;
using BocciaCoaching.Services.Interfaces;
using BocciaCoaching.Services.Wellness;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace BocciaCoaching.Services
{
    /// <summary>
    /// ES: Reglas del cuestionario diario de bienestar (McLean 2010): un registro por atleta por día
    /// local, semáforo con veto por ítem y permisos por rol. Los errores inesperados se propagan al
    /// <c>GlobalExceptionHandler</c>.
    /// </summary>
    public class WellnessService(IWellnessRepository repository, TimeProvider timeProvider) : IWellnessService
    {
        private const string AlreadyRegisteredMessage = "Ya registraste tu cuestionario de bienestar de hoy.";
        private const string AlreadyRegisteredForAthleteMessage = "El atleta ya tiene registrado el cuestionario de ese día.";
        private const string NotFoundMessage = "No se encontró el cuestionario de bienestar indicado.";
        private const string NoAccessMessage = "No tienes acceso al bienestar de este atleta.";

        public WellnessQuestionnaireDto GetQuestionnaire() => WellnessReadinessCalculator.Questionnaire;

        public async Task<ResponseContract<WellnessTodayStatusDto>> GetMyTodayStatusAsync(WellnessRequester requester, CancellationToken cancellationToken = default)
        {
            var today = Today(requester);
            var assessment = await repository.GetByAthleteAndDateAsync(requester.UserId, today, cancellationToken);

            var status = new WellnessTodayStatusDto
            {
                Date = today,
                TimeZone = requester.TimeZone.Id,
                Required = requester.IsAthlete,
                Completed = assessment is not null,
                Assessment = WithFeedback(assessment)
            };

            var message = status.MustComplete
                ? "Debes responder el cuestionario de bienestar de hoy."
                : status.Completed ? "Ya respondiste el cuestionario de hoy." : "No tienes cuestionario pendiente.";
            return ResponseContract<WellnessTodayStatusDto>.Ok(status, message);
        }

        public Task<ResponseContract<ResponseDailyWellnessDto>> SubmitMyWellnessAsync(WellnessRequester requester, SubmitMyWellnessDto dto, CancellationToken cancellationToken = default)
        {
            if (!requester.IsAthlete)
                return Task.FromResult(ResponseContract<ResponseDailyWellnessDto>.Forbidden(
                    "Solo los atletas registran su propio cuestionario de bienestar."));

            return CreateAsync(requester.UserId, dto.TeamId, Today(requester), dto, AlreadyRegisteredMessage, cancellationToken);
        }

        public async Task<ResponseContract<ResponseDailyWellnessDto>> RegisterDailyWellnessAsync(WellnessRequester requester, AddDailyWellnessDto dto, CancellationToken cancellationToken = default)
        {
            var today = Today(requester);
            var isSelf = dto.AthleteId == requester.UserId;

            if (!isSelf && !await CanManageAthleteAsync(requester, dto.AthleteId, cancellationToken))
                return ResponseContract<ResponseDailyWellnessDto>.Forbidden(
                    "Solo puedes registrar tu propio cuestionario o el de atletas de tus equipos.");

            var day = (dto.AssessmentDate ?? today).Date;
            if (day > today)
                return ResponseContract<ResponseDailyWellnessDto>.Invalid("No se puede registrar un cuestionario de un día futuro.");

            // El atleta solo registra el de hoy; entrenador y admin pueden registrar días pasados.
            var canBackdate = requester.IsAdmin || (requester.IsCoach && !isSelf);
            if (day != today && !canBackdate)
                return ResponseContract<ResponseDailyWellnessDto>.Invalid("Solo puedes registrar el cuestionario de hoy.");

            return await CreateAsync(dto.AthleteId, dto.TeamId, day, dto,
                isSelf ? AlreadyRegisteredMessage : AlreadyRegisteredForAthleteMessage, cancellationToken);
        }

        public async Task<ResponseContract<ResponseDailyWellnessDto>> UpdateDailyWellnessAsync(WellnessRequester requester, UpdateDailyWellnessDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await repository.GetForUpdateAsync(dto.DailyWellnessId, cancellationToken);
            if (entity is null)
                return ResponseContract<ResponseDailyWellnessDto>.NotFound(NotFoundMessage);

            var isOwner = entity.AthleteId == requester.UserId;
            var canManage = !isOwner && await CanManageAthleteAsync(requester, entity.AthleteId, cancellationToken);

            if (!isOwner && !canManage && !requester.IsAdmin)
                return ResponseContract<ResponseDailyWellnessDto>.Forbidden(NoAccessMessage);

            if (isOwner && !requester.IsAdmin && entity.AssessmentDate.Date != Today(requester))
                return ResponseContract<ResponseDailyWellnessDto>.Forbidden("Solo puedes corregir el cuestionario de hoy.");

            var evaluation = WellnessReadinessCalculator.Evaluate(dto.Fatigue, dto.Sleep, dto.MusclePain, dto.Stress, dto.Mood);
            Apply(entity, dto, evaluation);
            entity.ScaleVersion = DailyWellness.McLeanScaleVersion; // corregir un registro antiguo lo pasa a McLean
            entity.UpdatedAt = DateTime.UtcNow;
            await repository.SaveChangesAsync(cancellationToken);

            var updated = await repository.GetByIdAsync(entity.DailyWellnessId, cancellationToken);
            return ResponseContract<ResponseDailyWellnessDto>.Ok(WithFeedback(updated)!, "Cuestionario de bienestar actualizado");
        }

        public async Task<ResponseContract<ResponseDailyWellnessDto>> GetTodayWellnessAsync(WellnessRequester requester, Guid athleteId, CancellationToken cancellationToken = default)
            => await GetWellnessByDateAsync(requester, athleteId, Today(requester), cancellationToken);

        public async Task<ResponseContract<ResponseDailyWellnessDto>> GetWellnessByDateAsync(WellnessRequester requester, Guid athleteId, DateTime date, CancellationToken cancellationToken = default)
        {
            if (!await CanReadAthleteAsync(requester, athleteId, cancellationToken))
                return ResponseContract<ResponseDailyWellnessDto>.Forbidden(NoAccessMessage);

            var result = await repository.GetByAthleteAndDateAsync(athleteId, date, cancellationToken);
            return result is null
                ? ResponseContract<ResponseDailyWellnessDto>.NotFound("El atleta no ha registrado su cuestionario de bienestar para esta fecha.")
                : ResponseContract<ResponseDailyWellnessDto>.Ok(WithFeedback(result)!);
        }

        public async Task<ResponseContract<ResponseDailyWellnessDto>> GetWellnessByIdAsync(WellnessRequester requester, Guid dailyWellnessId, CancellationToken cancellationToken = default)
        {
            var result = await repository.GetByIdAsync(dailyWellnessId, cancellationToken);
            if (result is null)
                return ResponseContract<ResponseDailyWellnessDto>.NotFound(NotFoundMessage);

            if (!await CanReadAthleteAsync(requester, result.AthleteId, cancellationToken))
                return ResponseContract<ResponseDailyWellnessDto>.Forbidden(NoAccessMessage);

            return ResponseContract<ResponseDailyWellnessDto>.Ok(WithFeedback(result)!);
        }

        public async Task<ResponseContract<WellnessAthleteHistoryDto>> GetAthleteHistoryAsync(WellnessRequester requester, Guid athleteId, CancellationToken cancellationToken = default)
        {
            if (!await CanReadAthleteAsync(requester, athleteId, cancellationToken))
                return ResponseContract<WellnessAthleteHistoryDto>.Forbidden(NoAccessMessage);

            var result = await repository.GetAthleteHistoryAsync(athleteId, cancellationToken);
            if (result is null)
                return ResponseContract<WellnessAthleteHistoryDto>.NotFound("No se encontró el atleta indicado.");

            result.Records.ForEach(r => r.Feedback = WellnessReadinessCalculator.FeedbackFor(r));
            return ResponseContract<WellnessAthleteHistoryDto>.Ok(result);
        }

        public async Task<ResponseContract<List<ResponseDailyWellnessDto>>> GetTeamWellnessByDateAsync(WellnessRequester requester, Guid teamId, DateTime? date, CancellationToken cancellationToken = default)
        {
            var access = await CheckTeamAccessAsync<List<ResponseDailyWellnessDto>>(requester, teamId, cancellationToken);
            if (access is not null)
                return access;

            var result = await repository.GetTeamWellnessByDateAsync(teamId, (date ?? Today(requester)).Date, cancellationToken);
            result.ForEach(r => r.Feedback = WellnessReadinessCalculator.FeedbackFor(r));
            return ResponseContract<List<ResponseDailyWellnessDto>>.Ok(result);
        }

        public async Task<ResponseContract<TeamWellnessSummaryDto>> GetTeamSummaryAsync(WellnessRequester requester, Guid teamId, DateTime? date, CancellationToken cancellationToken = default)
        {
            var access = await CheckTeamAccessAsync<TeamWellnessSummaryDto>(requester, teamId, cancellationToken);
            if (access is not null)
                return access;

            var day = (date ?? Today(requester)).Date;
            var athletes = await repository.GetTeamAthletesAsync(teamId, cancellationToken);
            var assessments = await repository.GetTeamWellnessByDateAsync(teamId, day, cancellationToken);
            assessments.ForEach(r => r.Feedback = WellnessReadinessCalculator.FeedbackFor(r));

            var completedIds = assessments.Select(a => a.AthleteId).ToHashSet();
            var pending = athletes.Where(a => !completedIds.Contains(a.AthleteId)).OrderBy(a => a.AthleteName).ToList();

            var summary = new TeamWellnessSummaryDto
            {
                TeamId = teamId,
                Date = day,
                TotalAthletes = athletes.Count,
                Completed = assessments.Count,
                Pending = pending.Count,
                Green = assessments.Count(a => a.ReadinessLevel == nameof(ReadinessLevel.Green)),
                Yellow = assessments.Count(a => a.ReadinessLevel == nameof(ReadinessLevel.Yellow)),
                Red = assessments.Count(a => a.ReadinessLevel == nameof(ReadinessLevel.Red)),
                Assessments = assessments
                    .OrderBy(a => SeverityOrder(a.ReadinessLevel))
                    .ThenBy(a => a.TotalScore)
                    .ToList(),
                PendingAthletes = pending
            };

            return ResponseContract<TeamWellnessSummaryDto>.Ok(summary);
        }

        // ---------------- helpers ----------------

        private async Task<ResponseContract<ResponseDailyWellnessDto>> CreateAsync(
            Guid athleteId, Guid? teamId, DateTime day, WellnessAnswersDto answers, string duplicateMessage, CancellationToken cancellationToken)
        {
            if (!WellnessReadinessCalculator.AreValid(answers.Fatigue, answers.Sleep, answers.MusclePain, answers.Stress, answers.Mood))
                return ResponseContract<ResponseDailyWellnessDto>.Invalid("Todos los ítems deben estar entre 1 y 5.");

            if (await repository.ExistsForDayAsync(athleteId, day, cancellationToken))
                return ResponseContract<ResponseDailyWellnessDto>.Conflict(duplicateMessage);

            if (!await repository.AthleteExistsAsync(athleteId, cancellationToken))
                return ResponseContract<ResponseDailyWellnessDto>.NotFound("El atleta indicado no existe.");

            var evaluation = WellnessReadinessCalculator.Evaluate(answers.Fatigue, answers.Sleep, answers.MusclePain, answers.Stress, answers.Mood);
            var entity = new DailyWellness
            {
                AthleteId = athleteId,
                TeamId = teamId,
                AssessmentDate = day,
                ScaleVersion = DailyWellness.McLeanScaleVersion,
                CreatedAt = DateTime.UtcNow
            };
            Apply(entity, answers, evaluation);

            try
            {
                await repository.AddAsync(entity, cancellationToken);
            }
            catch (DbUpdateException ex) when (IsDuplicateKey(ex))
            {
                // Dos envíos simultáneos: el índice único (atleta, día) garantiza un solo registro.
                return ResponseContract<ResponseDailyWellnessDto>.Conflict(duplicateMessage);
            }

            var created = await repository.GetByIdAsync(entity.DailyWellnessId, cancellationToken);
            return ResponseContract<ResponseDailyWellnessDto>.Created(WithFeedback(created)!, "Cuestionario de bienestar registrado");
        }

        private static void Apply(DailyWellness entity, WellnessAnswersDto answers, WellnessEvaluation evaluation)
        {
            entity.Fatigue = answers.Fatigue;
            entity.Sleep = answers.Sleep;
            entity.MusclePain = answers.MusclePain;
            entity.Stress = answers.Stress;
            entity.Mood = answers.Mood;
            entity.Observations = answers.Observations;
            entity.TotalScore = evaluation.TotalScore;
            entity.AverageScore = evaluation.AverageScore;
            entity.ReadinessLevel = evaluation.Level.ToString();
            entity.VetoApplied = evaluation.VetoApplied;
        }

        private static ResponseDailyWellnessDto? WithFeedback(ResponseDailyWellnessDto? record)
        {
            if (record is not null)
                record.Feedback = WellnessReadinessCalculator.FeedbackFor(record);
            return record;
        }

        /// <summary>ES: "Hoy" en la zona horaria del atleta (no la del servidor, que corre en UTC).</summary>
        private DateTime Today(WellnessRequester requester)
            => TimeZoneInfo.ConvertTime(timeProvider.GetUtcNow(), requester.TimeZone).Date;

        private async Task<bool> CanReadAthleteAsync(WellnessRequester requester, Guid athleteId, CancellationToken cancellationToken)
            => athleteId == requester.UserId || await CanManageAthleteAsync(requester, athleteId, cancellationToken);

        /// <summary>ES: Admin, o entrenador de algún equipo del atleta.</summary>
        private async Task<bool> CanManageAthleteAsync(WellnessRequester requester, Guid athleteId, CancellationToken cancellationToken)
            => requester.IsAdmin ||
               (requester.IsCoach && await repository.IsCoachOfAthleteAsync(requester.UserId, athleteId, cancellationToken));

        private async Task<ResponseContract<T>?> CheckTeamAccessAsync<T>(WellnessRequester requester, Guid teamId, CancellationToken cancellationToken)
        {
            if (!await repository.TeamExistsAsync(teamId, cancellationToken))
                return ResponseContract<T>.NotFound("El equipo no existe.");

            if (!requester.IsAdmin && !await repository.IsCoachOfTeamAsync(requester.UserId, teamId, cancellationToken))
                return ResponseContract<T>.Forbidden("Solo el entrenador del equipo puede ver el bienestar del equipo.");

            return null;
        }

        private static int SeverityOrder(string? level) => level switch
        {
            nameof(ReadinessLevel.Red) => 0,
            nameof(ReadinessLevel.Yellow) => 1,
            nameof(ReadinessLevel.Green) => 2,
            _ => 3
        };

        private static bool IsDuplicateKey(DbUpdateException ex)
            => ex.InnerException is MySqlException { ErrorCode: MySqlErrorCode.DuplicateKeyEntry };
    }
}
