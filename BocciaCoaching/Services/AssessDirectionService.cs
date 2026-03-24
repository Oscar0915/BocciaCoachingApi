using BocciaCoaching.Models.DTO.AssessDirection;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Notification;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IAssessDirection;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class AssessDirectionService : IAssessDirectionService
    {
        private readonly IAssessDirectionRepository _assessDirectionRepository;
        private readonly ITeamValidationRepository _teamValidationRepository;
        private readonly IValidationsAssetsDirection _validationsAssetsDirection;
        private readonly INotificationService _notificationService;

        public AssessDirectionService(
            IAssessDirectionRepository assessDirectionRepository,
            ITeamValidationRepository teamValidationRepository,
            IValidationsAssetsDirection validationsAssetsDirection,
            INotificationService notificationService)
        {
            _assessDirectionRepository = assessDirectionRepository;
            _teamValidationRepository = teamValidationRepository;
            _validationsAssetsDirection = validationsAssetsDirection;
            _notificationService = notificationService;
        }

        public async Task<ResponseContract<AthletesToEvaluatedDirection>> AgregarAtletaAEvaluacion(
            RequestAddAthleteToDirectionEvaluationDto athletesToEvaluated)
        {
            return await _assessDirectionRepository.AgregarAtletaAEvaluacion(athletesToEvaluated);
        }

        public async Task<bool> AgregarDetalleDeEvaluacion(RequestAddDetailToDirectionEvaluation request)
        {
            var isUpdateDetail =
                await _validationsAssetsDirection.IsUpdateDetailAssessDirection(request);

            await _assessDirectionRepository.AgregarDetalleDeEvaluacion(request, isUpdateDetail);

            var dataDirectionStatistic = new DirectionStatistics();

            // En control de dirección, son 24 lanzamientos:
            // 8 lanzamientos a 3 metros, 8 a 6 metros, 8 a 9 metros
            // 4 desde box derecho y 4 desde box izquierdo por cada distancia
            if (request.ThrowOrder == 24)
            {
                // Consultamos toda la evaluación del atleta
                var listDetailsEvaluation = await _assessDirectionRepository.GetAllDetailsEvaluation(request);

                // Puntaje máximo posible: 24 lanzamientos * 5 puntos max = 120
                // Calculamos la precisión general (porcentaje del puntaje máximo)
                dataDirectionStatistic.EffectivenessPercentage = (double)listDetailsEvaluation.Sum(l => l.ScoreObtained)! / 120;

                // Calculamos los lanzamientos efectivos (score >= 3)
                var hit = listDetailsEvaluation.Where(l => l.ScoreObtained >= 3).ToList();
                dataDirectionStatistic.EffectiveThrow = hit.Count;
                dataDirectionStatistic.AccuracyPercentage = (double)hit.Count / 24;

                // Calculamos los lanzamientos fallidos
                var misses = listDetailsEvaluation.Where(l => l.ScoreObtained < 3).ToList();
                dataDirectionStatistic.FailedThrow = misses.Count;

                // Lanzamientos efectivos a distancia corta (3 metros) - 8 lanzamientos
                var shortThrows = listDetailsEvaluation
                    .Where(l => l.TargetDistance == 3 && l.ScoreObtained >= 3).ToList();
                dataDirectionStatistic.ShortThrow = shortThrows.Count;

                // Lanzamientos efectivos a distancia media (6 metros) - 8 lanzamientos
                var mediumThrows = listDetailsEvaluation
                    .Where(l => l.TargetDistance == 6 && l.ScoreObtained >= 3).ToList();
                dataDirectionStatistic.MediumThrow = mediumThrows.Count;

                // Lanzamientos efectivos a distancia larga (9 metros) - 8 lanzamientos
                var longThrows = listDetailsEvaluation
                    .Where(l => l.TargetDistance == 9 && l.ScoreObtained >= 3).ToList();
                dataDirectionStatistic.LongThrow = longThrows.Count;

                // Porcentaje de efectividad por distancia (de 8 lanzamientos por distancia)
                dataDirectionStatistic.ShortEffectivenessPercentage = (double)dataDirectionStatistic.ShortThrow / 8;
                dataDirectionStatistic.MediumEffectivenessPercentage = (double)dataDirectionStatistic.MediumThrow / 8;
                dataDirectionStatistic.LongEffectivenessPercentage = dataDirectionStatistic.LongThrow / 8;

                // Precisión de lanzamiento por distancia (suma de puntajes obtenidos)
                var shortRangeAccuracy = listDetailsEvaluation
                    .Where(l => l.TargetDistance == 3)
                    .Select(l => l.ScoreObtained).Sum();
                dataDirectionStatistic.ShortThrowAccuracy = Convert.ToInt32(shortRangeAccuracy);

                var mediumRangeAccuracy = listDetailsEvaluation
                    .Where(l => l.TargetDistance == 6)
                    .Select(l => l.ScoreObtained).Sum();
                dataDirectionStatistic.MediumThrowAccuracy = Convert.ToInt32(mediumRangeAccuracy);

                var longRangeAccuracy = listDetailsEvaluation
                    .Where(l => l.TargetDistance == 9)
                    .Select(l => l.ScoreObtained).Sum();
                dataDirectionStatistic.LongThrowAccuracy = Convert.ToInt32(longRangeAccuracy);

                // Porcentaje de precisión por distancia (puntaje max por distancia: 8 * 5 = 40)
                dataDirectionStatistic.ShortAccuracyPercentage = (double)shortRangeAccuracy! / 40;
                dataDirectionStatistic.MediumAccuracyPercentage = (double)mediumRangeAccuracy! / 40;
                dataDirectionStatistic.LongAccuracyPercentage = (double)longRangeAccuracy! / 40;

                // ====== Estadísticas de desviación ======
                // Total de desviaciones
                dataDirectionStatistic.TotalDeviatedRight = listDetailsEvaluation.Count(l => l.DeviatedRight);
                dataDirectionStatistic.TotalDeviatedLeft = listDetailsEvaluation.Count(l => l.DeviatedLeft);

                // Porcentaje de desviación general
                dataDirectionStatistic.DeviatedRightPercentage = (double)dataDirectionStatistic.TotalDeviatedRight / 24;
                dataDirectionStatistic.DeviatedLeftPercentage = (double)dataDirectionStatistic.TotalDeviatedLeft / 24;

                // Desviaciones por distancia corta (3m)
                dataDirectionStatistic.ShortDeviatedRight = listDetailsEvaluation
                    .Count(l => l.TargetDistance == 3 && l.DeviatedRight);
                dataDirectionStatistic.ShortDeviatedLeft = listDetailsEvaluation
                    .Count(l => l.TargetDistance == 3 && l.DeviatedLeft);

                // Desviaciones por distancia media (6m)
                dataDirectionStatistic.MediumDeviatedRight = listDetailsEvaluation
                    .Count(l => l.TargetDistance == 6 && l.DeviatedRight);
                dataDirectionStatistic.MediumDeviatedLeft = listDetailsEvaluation
                    .Count(l => l.TargetDistance == 6 && l.DeviatedLeft);

                // Desviaciones por distancia larga (9m)
                dataDirectionStatistic.LongDeviatedRight = listDetailsEvaluation
                    .Count(l => l.TargetDistance == 9 && l.DeviatedRight);
                dataDirectionStatistic.LongDeviatedLeft = listDetailsEvaluation
                    .Count(l => l.TargetDistance == 9 && l.DeviatedLeft);

                dataDirectionStatistic.AthleteId = request.AthleteId;
                dataDirectionStatistic.AssessDirectionId = request.AssessDirectionId;

                // Guardamos las estadísticas
                await _assessDirectionRepository.InsertDirectionTestStats(dataDirectionStatistic);

                // Obtener el CoachId para enviar la notificación
                var coachId = await _assessDirectionRepository.GetCoachIdByAssessmentAsync(request.AssessDirectionId);

                if (coachId.HasValue)
                {
                    var notificationMessage = new RequestCreateNotificationMessageDto
                    {
                        Message = "Tu evaluación de control de dirección ha sido completada. Revisa tus estadísticas.",
                        SenderId = coachId.Value,
                        ReceiverId = request.AthleteId,
                        NotificationTypeId = 2,
                        Status = true
                    };

                    await _notificationService.CreateMessage(notificationMessage);
                }
            }

            return true;
        }

        public async Task<ResponseContract<ResponseAddAssessDirectionDto>> CreateEvaluation(
            AddAssessDirectionDto addAssessDirectionDto)
        {
            try
            {
                var isValidTeam = await _teamValidationRepository.ValidateTeam(new Team
                {
                    TeamId = addAssessDirectionDto.TeamId
                });

                if (!isValidTeam)
                {
                    return ResponseContract<ResponseAddAssessDirectionDto>.Fail(
                        "El equipo no está activo"
                    );
                }

                var repoResult = await _assessDirectionRepository.CreateAssessmentIfNoneActiveAsync(addAssessDirectionDto);
                if (!repoResult.Success)
                {
                    return ResponseContract<ResponseAddAssessDirectionDto>.Fail(repoResult.Message);
                }

                return ResponseContract<ResponseAddAssessDirectionDto>.Ok(repoResult.Data, repoResult.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<ResponseAddAssessDirectionDto>.Fail(
                    $"Error al crear la evaluación de dirección: {e.Message}"
                );
            }
        }

        public async Task<ResponseContract<ActiveDirectionEvaluationDto>> GetActiveEvaluationWithDetails(int teamId, int coachId)
        {
            try
            {
                var isValidTeam = await _teamValidationRepository.ValidateTeam(new Team { TeamId = teamId });
                if (!isValidTeam)
                {
                    return ResponseContract<ActiveDirectionEvaluationDto>.Fail("El equipo no existe o no está activo");
                }

                var activeEvaluation = await _assessDirectionRepository.GetActiveEvaluationWithDetailsAsync(teamId, coachId);

                if (activeEvaluation == null)
                {
                    return ResponseContract<ActiveDirectionEvaluationDto>.Fail(
                        "No hay evaluación de dirección activa para este equipo");
                }

                return ResponseContract<ActiveDirectionEvaluationDto>.Ok(activeEvaluation, "Evaluación de dirección activa encontrada");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<ActiveDirectionEvaluationDto>.Fail(
                    $"Error al obtener la evaluación activa: {e.Message}");
            }
        }

        public async Task<object> GetEvaluationDebugInfo(int teamId)
        {
            try
            {
                return await _assessDirectionRepository.GetEvaluationDebugInfoAsync(teamId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new { Error = e.Message };
            }
        }

        public async Task<ResponseContract<bool>> UpdateEvaluationState(UpdateAssessDirectionDto updateDto)
        {
            try
            {
                if (updateDto.State != "A" && updateDto.State != "T" && updateDto.State != "C")
                {
                    return ResponseContract<bool>.Fail(
                        "Estado inválido. Los estados válidos son: A (Activa), T (Terminada), C (Cancelada)");
                }

                return await _assessDirectionRepository.UpdateState(updateDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<bool>.Fail($"Error al actualizar el estado: {e.Message}");
            }
        }

        public async Task<ResponseContract<List<DirectionEvaluationSummaryDto>>> GetTeamEvaluations(int teamId)
        {
            try
            {
                var isValidTeam = await _teamValidationRepository.ValidateTeam(new Team { TeamId = teamId });
                if (!isValidTeam)
                {
                    return ResponseContract<List<DirectionEvaluationSummaryDto>>.Fail(
                        "El equipo no existe o no está activo");
                }

                var evaluations = await _assessDirectionRepository.GetTeamEvaluationsAsync(teamId);

                if (evaluations.Count == 0)
                {
                    return ResponseContract<List<DirectionEvaluationSummaryDto>>.Ok(
                        new List<DirectionEvaluationSummaryDto>(),
                        "No se encontraron evaluaciones de dirección para este equipo"
                    );
                }

                return ResponseContract<List<DirectionEvaluationSummaryDto>>.Ok(
                    evaluations,
                    $"Se encontraron {evaluations.Count} evaluaciones de dirección"
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<DirectionEvaluationSummaryDto>>.Fail(
                    $"Error al obtener las evaluaciones: {e.Message}");
            }
        }

        public async Task<ResponseContract<List<DirectionAthleteStatisticsDto>>> GetEvaluationStatistics(
            int assessDirectionId)
        {
            try
            {
                var statistics = await _assessDirectionRepository.GetEvaluationStatisticsAsync(assessDirectionId);

                if (statistics.Count == 0)
                {
                    return ResponseContract<List<DirectionAthleteStatisticsDto>>.Fail(
                        "No se encontraron estadísticas para esta evaluación de dirección. La evaluación debe estar terminada para tener estadísticas."
                    );
                }

                return ResponseContract<List<DirectionAthleteStatisticsDto>>.Ok(
                    statistics,
                    $"Se encontraron estadísticas de {statistics.Count} atletas"
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<DirectionAthleteStatisticsDto>>.Fail(
                    $"Error al obtener las estadísticas: {e.Message}");
            }
        }

        public async Task<ResponseContract<DirectionEvaluationDetailsDto>> GetEvaluationDetails(
            int assessDirectionId)
        {
            try
            {
                var details = await _assessDirectionRepository.GetEvaluationDetailsAsync(assessDirectionId);

                if (details == null)
                {
                    return ResponseContract<DirectionEvaluationDetailsDto>.Fail(
                        "No se encontró la evaluación de dirección especificada");
                }

                return ResponseContract<DirectionEvaluationDetailsDto>.Ok(details,
                    "Detalles de evaluación de dirección obtenidos correctamente");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<DirectionEvaluationDetailsDto>.Fail(
                    $"Error al obtener los detalles: {e.Message}");
            }
        }

        public async Task<ResponseContract<bool>> CancelEvaluation(CancelAssessDirectionDto cancelDto)
        {
            try
            {
                var evaluationDetails =
                    await _assessDirectionRepository.GetEvaluationDetailsAsync(cancelDto.AssessDirectionId);
                if (evaluationDetails == null)
                    return ResponseContract<bool>.Fail("No se encontró la evaluación de dirección especificada");

                if (evaluationDetails.State != "A")
                    return ResponseContract<bool>.Fail(
                        "Sólo se puede cancelar una evaluación que esté en estado Activa (A)");

                if (evaluationDetails.CoachId != cancelDto.CoachId)
                    return ResponseContract<bool>.Fail(
                        "No autorizado: sólo el entrenador que creó la evaluación puede cancelarla");

                var athleteIds = evaluationDetails.Athletes.Select(a => a.AthleteId).Distinct().ToList();

                var repoResult = await _assessDirectionRepository.CancelAssessmentAsync(
                    cancelDto.AssessDirectionId, cancelDto.CoachId, cancelDto.Reason);
                if (!repoResult.Success)
                    return ResponseContract<bool>.Fail(repoResult.Message);

                // Enviar notificaciones a los atletas
                foreach (var athleteId in athleteIds)
                {
                    var notificationMessage = new RequestCreateNotificationMessageDto
                    {
                        Message =
                            $"La evaluación de control de dirección (ID: {cancelDto.AssessDirectionId}) ha sido cancelada. Motivo: {cancelDto.Reason ?? "No especificado"}",
                        SenderId = cancelDto.CoachId,
                        ReceiverId = athleteId,
                        NotificationTypeId = 3,
                        Status = true,
                        ReferenceId = cancelDto.AssessDirectionId
                    };

                    try
                    {
                        await _notificationService.CreateMessage(notificationMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error enviando notificación a atleta {athleteId}: {ex.Message}");
                    }
                }

                return ResponseContract<bool>.Ok(true, "Evaluación de dirección cancelada correctamente");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error en CancelEvaluation: {e.Message}");
                return ResponseContract<bool>.Fail($"Error al cancelar la evaluación: {e.Message}");
            }
        }
    }
}

