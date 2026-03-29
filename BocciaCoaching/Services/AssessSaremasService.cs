using BocciaCoaching.Models.DTO.AssessSaremas;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Notification;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IAssessSaremas;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class AssessSaremasService : IAssessSaremasService
    {
        private readonly IAssessSaremasRepository _repository;
        private readonly IValidationsAssessSaremas _validations;
        private readonly ITeamValidationRepository _teamValidation;
        private readonly INotificationService _notificationService;

        // Mapeo estático de los 28 tiros SAREMAS+
        private static readonly (string Diagonal, string Component)[] ThrowMap = new[]
        {
            // Bloque 1 (Roja): tiros 1-7
            ("Roja", "Salida"), ("Roja", "Romper"), ("Roja", "Arrimar"),
            ("Roja", "Empujar A"), ("Roja", "Sapito Ras"), ("Roja", "Montar"), ("Roja", "Penal"),
            // Bloque 2 (Azul): tiros 8-14
            ("Azul", "Romper"), ("Azul", "Arrimar"), ("Azul", "Empujar F"),
            ("Azul", "Romper AE"), ("Azul", "Apoyar"), ("Azul", "Empujar A"), ("Azul", "Penal"),
            // Bloque 3 (Roja): tiros 15-21
            ("Roja", "Romper"), ("Roja", "Arrimar"), ("Roja", "Empujar A"),
            ("Roja", "Empujar LA"), ("Roja", "Sapito AE"), ("Roja", "Arrimar"), ("Roja", "Penal"),
            // Bloque 4 (Azul): tiros 22-28
            ("Azul", "Salida"), ("Azul", "Romper"), ("Azul", "Arrimar"),
            ("Azul", "Empujar A"), ("Azul", "Arrima R Zona"), ("Azul", "Libre Entrega"), ("Azul", "Penal")
        };

        public AssessSaremasService(
            IAssessSaremasRepository repository,
            IValidationsAssessSaremas validations,
            ITeamValidationRepository teamValidation,
            INotificationService notificationService)
        {
            _repository = repository;
            _validations = validations;
            _teamValidation = teamValidation;
            _notificationService = notificationService;
        }

        public async Task<ResponseContract<ResponseAddSaremasDto>> CreateEvaluation(AddSaremasEvaluationDto dto)
        {
            try
            {
                var isValidTeam = await _teamValidation.ValidateTeam(new Team { TeamId = dto.TeamId });
                if (!isValidTeam)
                    return ResponseContract<ResponseAddSaremasDto>.Fail("El equipo no está activo");

                return await _repository.CreateEvaluationIfNoneActiveAsync(dto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<ResponseAddSaremasDto>.Fail($"Error al crear la evaluación SAREMAS+: {e.Message}");
            }
        }

        public async Task<ResponseContract<SaremasAthleteEvaluation>> AddAthleteToEvaluation(RequestAddAthleteToSaremasDto dto)
        {
            return await _repository.AddAthleteToEvaluationAsync(dto);
        }

        public async Task<ResponseContract<bool>> AddThrowDetail(RequestAddSaremasDetailDto dto)
        {
            try
            {
                // Validar rango de puntaje
                if (dto.ScoreObtained < 0 || dto.ScoreObtained > 5)
                    return ResponseContract<bool>.Fail("El puntaje debe estar entre 0 y 5");

                // Validar número de tiro
                if (dto.ThrowNumber < 1 || dto.ThrowNumber > 28)
                    return ResponseContract<bool>.Fail("El número de tiro debe estar entre 1 y 28");

                // Validar observaciones obligatorias si puntaje <= 2
                if (dto.ScoreObtained <= 2 && string.IsNullOrWhiteSpace(dto.Observations))
                    return ResponseContract<bool>.Fail("Las observaciones son obligatorias cuando el puntaje es ≤ 2");

                // Verificar si es update o insert
                var isUpdate = await _validations.IsThrowDuplicateAsync(dto);

                var result = await _repository.AddThrowDetailAsync(dto, isUpdate);
                if (!result)
                    return ResponseContract<bool>.Fail("Error al registrar el lanzamiento");

                // Si se completaron los 28 tiros, calcular estadísticas
                if (dto.ThrowNumber == 28)
                {
                    var allThrows = await _repository.GetAllThrowsForAthleteAsync(dto.SaremasEvalId, dto.AthleteId);
                    if (allThrows.Count >= 28)
                    {
                        var totalScore = allThrows.Sum(t => t.ScoreObtained);
                        var averageScore = allThrows.Average(t => (double)t.ScoreObtained);

                        await _repository.UpdateEvaluationScoresAsync(dto.SaremasEvalId, totalScore, Math.Round(averageScore, 2));

                        // Notificación al atleta
                        var coachId = await _repository.GetCoachIdByEvaluationAsync(dto.SaremasEvalId);
                        if (coachId.HasValue)
                        {
                            var notification = new RequestCreateNotificationMessageDto
                            {
                                Message = "Tu evaluación SAREMAS+ ha sido completada. Revisa tus estadísticas.",
                                SenderId = coachId.Value,
                                ReceiverId = dto.AthleteId,
                                NotificationTypeId = 2,
                                Status = true
                            };
                            await _notificationService.CreateMessage(notification);
                        }
                    }
                }

                return ResponseContract<bool>.Ok(true, isUpdate ? "Lanzamiento actualizado" : "Lanzamiento registrado");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<bool>.Fail($"Error al registrar el lanzamiento: {e.Message}");
            }
        }

        public async Task<ResponseContract<ActiveSaremasEvaluationDto>> GetActiveEvaluation(int teamId, int coachId)
        {
            try
            {
                var isValidTeam = await _teamValidation.ValidateTeam(new Team { TeamId = teamId });
                if (!isValidTeam)
                    return ResponseContract<ActiveSaremasEvaluationDto>.Fail("El equipo no existe o no está activo");

                var active = await _repository.GetActiveEvaluationAsync(teamId, coachId);
                if (active == null)
                    return ResponseContract<ActiveSaremasEvaluationDto>.Fail("No hay evaluación SAREMAS+ activa para este equipo");

                return ResponseContract<ActiveSaremasEvaluationDto>.Ok(active, "Evaluación activa encontrada");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<ActiveSaremasEvaluationDto>.Fail($"Error al obtener la evaluación activa: {e.Message}");
            }
        }

        public async Task<ResponseContract<bool>> UpdateState(UpdateSaremasStateDto dto)
        {
            try
            {
                var validStates = new[] { "Active", "Completed", "Cancelled" };
                if (!validStates.Contains(dto.State))
                    return ResponseContract<bool>.Fail("Estado inválido. Los estados válidos son: Active, Completed, Cancelled");

                return await _repository.UpdateStateAsync(dto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<bool>.Fail($"Error al actualizar el estado: {e.Message}");
            }
        }

        public async Task<ResponseContract<bool>> CancelEvaluation(CancelSaremasDto dto)
        {
            return await _repository.CancelAsync(dto.SaremasEvalId, dto.CoachId, dto.Reason);
        }

        public async Task<ResponseContract<List<SaremasEvaluationSummaryDto>>> GetTeamEvaluations(int teamId)
        {
            try
            {
                var isValidTeam = await _teamValidation.ValidateTeam(new Team { TeamId = teamId });
                if (!isValidTeam)
                    return ResponseContract<List<SaremasEvaluationSummaryDto>>.Fail("El equipo no existe o no está activo");

                var evaluations = await _repository.GetTeamEvaluationsAsync(teamId);
                return ResponseContract<List<SaremasEvaluationSummaryDto>>.Ok(
                    evaluations,
                    evaluations.Count > 0 ? $"Se encontraron {evaluations.Count} evaluaciones" : "No se encontraron evaluaciones");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<SaremasEvaluationSummaryDto>>.Fail($"Error al obtener evaluaciones: {e.Message}");
            }
        }

        public async Task<ResponseContract<SaremasEvaluationDetailsDto>> GetEvaluationDetails(int saremasEvalId)
        {
            try
            {
                var details = await _repository.GetEvaluationDetailsAsync(saremasEvalId);
                if (details == null)
                    return ResponseContract<SaremasEvaluationDetailsDto>.Fail("Evaluación no encontrada");

                return ResponseContract<SaremasEvaluationDetailsDto>.Ok(details, "Detalles obtenidos correctamente");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<SaremasEvaluationDetailsDto>.Fail($"Error al obtener detalles: {e.Message}");
            }
        }

        public async Task<ResponseContract<SaremasStatisticsDto>> GetEvaluationStatistics(int saremasEvalId)
        {
            try
            {
                var stats = await _repository.GetEvaluationStatisticsAsync(saremasEvalId);
                if (stats == null)
                    return ResponseContract<SaremasStatisticsDto>.Fail("No se encontraron estadísticas para esta evaluación");

                return ResponseContract<SaremasStatisticsDto>.Ok(stats, "Estadísticas obtenidas correctamente");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<SaremasStatisticsDto>.Fail($"Error al obtener estadísticas: {e.Message}");
            }
        }

        public async Task<ResponseContract<SaremasAthleteHistoryDto>> GetAthleteHistory(int athleteId)
        {
            try
            {
                var history = await _repository.GetAthleteHistoryAsync(athleteId);
                if (history == null)
                    return ResponseContract<SaremasAthleteHistoryDto>.Fail("Atleta no encontrado");

                return ResponseContract<SaremasAthleteHistoryDto>.Ok(history, "Historial obtenido correctamente");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<SaremasAthleteHistoryDto>.Fail($"Error al obtener el historial: {e.Message}");
            }
        }
    }
}

