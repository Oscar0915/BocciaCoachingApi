﻿using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Notification;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IAssesstStrength;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class AssessStrengthService: IAssessStrengthService
    {
        private readonly IAssessStrengthRepository _assessStrengthRepository;
        private readonly ITeamValidationRepository   _teamValidationRepository;
        private readonly IValidationsAssetsStrength _validationsAssetsStrength;
        private readonly INotificationService _notificationService;


        // Constructor
        public AssessStrengthService(IAssessStrengthRepository assessStrengthRepository, ITeamValidationRepository teamValidationRepository,
            IValidationsAssetsStrength validationsAssetsStrength, INotificationService notificationService)
        {
            _assessStrengthRepository = assessStrengthRepository;
            _teamValidationRepository = teamValidationRepository;
            _validationsAssetsStrength = validationsAssetsStrength;
            _notificationService = notificationService;
        }
        public async Task<ResponseContract<AthletesToEvaluated>> AgregarAtletaAEvaluacion(RequestAddAthleteToEvaluationDto athletesToEvaluated)
        {
            return await _assessStrengthRepository.AgregarAtletaAEvaluacion(athletesToEvaluated);
        }

        public async Task<bool> AgregarDetalleDeEvaluacion(RequestAddDetailToEvaluationForAthlete requestAddDetailToEvaluationForAthlete)
        {
            var isUdateDetail =
                await _validationsAssetsStrength.IsUpdateDetailAssessStrength(requestAddDetailToEvaluationForAthlete);
           
            await _assessStrengthRepository.AgregarDetalleDeEvaluacion(requestAddDetailToEvaluationForAthlete,isUdateDetail);

            var dataStrenthStatistic = new StrengthStatistics();
            
            if (requestAddDetailToEvaluationForAthlete.ThrowOrder == 36)
            {
                //Consultamos toda la evaluación
                var listDetailsEvaluation= await _assessStrengthRepository.GetAllDetailsEvaluation(requestAddDetailToEvaluationForAthlete);
                
                //Calculamos la precisión
                dataStrenthStatistic.EffectivenessPercentage = (double) listDetailsEvaluation.Sum(l => l.ScoreObtained)!/180;
                
                //Calculamos los lanzamientos efectivos
                var hit = listDetailsEvaluation.Where(l =>
                    l.ScoreObtained >= 3).ToList();
                dataStrenthStatistic.EffectiveThrow = hit.Count;
                dataStrenthStatistic.AccuracyPercentage = (double)  hit.Count / 36;
                
                //Calculamos los lanzamientos fallidos 
                var misses = listDetailsEvaluation.Where(l =>
                    l.ScoreObtained < 3).ToList();
                dataStrenthStatistic.FailedThrow= misses.Count;
                
                //Calculamos los lanzamientos de 1.5 a 4.0 metros
                var shortThrows = listDetailsEvaluation.Where(l => (
                    l.TargetDistance >= (decimal?)1.5) && (l.TargetDistance <= 4) && (l.ScoreObtained >=3)).ToList();
                dataStrenthStatistic.ShortThrow = shortThrows.Count;
                
                //Calculamos los lanzamientos de 4.5 a 7.0 metros
                var mediumThrows = listDetailsEvaluation.Where(l => (
                    l.TargetDistance >= (decimal?)4.5) && (l.TargetDistance <= 7) && (l.ScoreObtained >=3)).ToList();
                dataStrenthStatistic.MediumThrow = mediumThrows.Count;
                
                //Calculamos los lanzamientos de 7.5 a 10.0 metros
                var  longThrows= listDetailsEvaluation.Where(l => (
                    l.TargetDistance >= (decimal?)7.5) && (l.TargetDistance <= 10) && (l.ScoreObtained >=3)).ToList();
                dataStrenthStatistic.LongThrow = longThrows.Count;
                
                //Calculamos porcentaje de efectividad corto
                dataStrenthStatistic.ShortEffectivenessPercentage = (double) dataStrenthStatistic.ShortThrow / 12;
                //Calculamos porcentaje de efectividad medio
                dataStrenthStatistic.MediumEffectivenessPercentage = (double)  dataStrenthStatistic.MediumThrow  / 12;
                //Calculamos porcentaje de efectividad largo
                dataStrenthStatistic.LongEffectivenessPercentage =   dataStrenthStatistic.LongThrow / 12;
                
                //Calculamos la precision de lanzamiento corto
                var shortRangeAccuracy = listDetailsEvaluation.Where(l => (
                    l.TargetDistance >= (decimal?)1.5) && (l.TargetDistance <= 4))
                    .Select(l=>l.ScoreObtained).ToList().Sum();
                dataStrenthStatistic.ShortThrowAccuracy= Convert.ToInt32(shortRangeAccuracy);
                //Calculamos la precision de lanzamiento medio
                var mediumRangeAccuracy = listDetailsEvaluation.Where(l => (
                        l.TargetDistance >= (decimal?)4.5) && (l.TargetDistance <= 7))
                    .Select(l=>l.ScoreObtained).ToList().Sum();
                dataStrenthStatistic.MediumThrowAccuracy= Convert.ToInt32(mediumRangeAccuracy);
                //Calculamos la precision de lanzamiento largo
                var longRangeAccuracy = listDetailsEvaluation.Where(l => (
                        l.TargetDistance >= (decimal?)7.5) && (l.TargetDistance <= 10))
                    .Select(l=>l.ScoreObtained).ToList().Sum();
                dataStrenthStatistic.LongThrowAccuracy= Convert.ToInt32(longRangeAccuracy);
                
                
                //Calculamos el porcentaje de precisión corto
                dataStrenthStatistic.ShortAccuracyPercentage = (double) shortRangeAccuracy! / 60;
                //Calculamos el porcentaje de precisión medio
                dataStrenthStatistic.MediumAccuracyPercentage = (double) mediumRangeAccuracy! / 60;
                //Calculamos el porcentaje de precisión largo
                dataStrenthStatistic.LongAccuracyPercentage = (double) longRangeAccuracy! / 60;

                dataStrenthStatistic.AthleteId = requestAddDetailToEvaluationForAthlete.AthleteId;
                dataStrenthStatistic.AssessStrengthId = requestAddDetailToEvaluationForAthlete.AssessStrengthId;
                
                //Guardamos las estadisticas
                await _assessStrengthRepository.InsertStrengthTestStats(dataStrenthStatistic);

                // Obtener el CoachId para enviar la notificación
                var coachId = await _assessStrengthRepository.GetCoachIdByAssessmentAsync(requestAddDetailToEvaluationForAthlete.AssessStrengthId);
                
                if (coachId.HasValue)
                {
                    // Crear notificación para el atleta
                    var notificationMessage = new RequestCreateNotificationMessageDto
                    {
                        Message = "Tu evaluación de fuerza ha sido completada. Revisa tus estadísticas.",
                        SenderId = coachId.Value,
                        ReceiverId = requestAddDetailToEvaluationForAthlete.AthleteId,
                        NotificationTypeId = 2, // Tipo de notificación para evaluación completada
                        Status = true
                    };

                    await _notificationService.CreateMessage(notificationMessage);
                }
            }

            return true;
        }

        /// <summary>
        /// Crear una nueva evaluación 
        /// </summary>
        /// <param name="addAssessStrengthDto"></param>
        /// <returns></returns>
        public async Task<ResponseContract<ResponseAddAssessStrengthDto>> CreateEvaluation(AddAssessStrengthDto addAssessStrengthDto)
        {
            try
            {
                var isValidTeam = await _teamValidationRepository.ValidateTeam(new Team
                {
                    TeamId = addAssessStrengthDto.TeamId
                });

                if (!isValidTeam)
                {
                    return ResponseContract<ResponseAddAssessStrengthDto>.Fail(
                        "El equipo no está activo"
                    );
                }
 
                // Llama al repositorio para crear la evaluación de forma atómica (verifica por TeamId)
                var repoResult = await _assessStrengthRepository.CreateAssessmentIfNoneActiveAsync(addAssessStrengthDto);
                if (repoResult == null || !repoResult.Success)
                {
                    return ResponseContract<ResponseAddAssessStrengthDto>.Fail(repoResult?.Message ?? "Error al crear la evaluación");
                }

                return ResponseContract<ResponseAddAssessStrengthDto>.Ok(repoResult.Data, repoResult.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
 
                   return ResponseContract<ResponseAddAssessStrengthDto>.Fail(
                      $"Error al crear la evaluación: {e.Message}"
                  );
                }
            }

        /// <summary>
        /// Obtiene la evaluación de fuerza activa para un equipo con todos sus detalles
        /// </summary>
        /// <param name="teamId">ID del equipo</param>
        /// <returns>Información completa de la evaluación activa o null si no hay ninguna</returns>
        public async Task<ResponseContract<ActiveEvaluationDto>> GetActiveEvaluationWithDetails(int teamId)
        {
            try
            {
                // Validar que el equipo existe
                var isValidTeam = await _teamValidationRepository.ValidateTeam(new Team { TeamId = teamId });
                if (!isValidTeam)
                {
                    return ResponseContract<ActiveEvaluationDto>.Fail("El equipo no existe o no está activo");
                }

                // Obtener la evaluación activa del repositorio
                var activeEvaluation = await _assessStrengthRepository.GetActiveEvaluationWithDetailsAsync(teamId);
                
                if (activeEvaluation == null)
                {
                    return ResponseContract<ActiveEvaluationDto>.Fail("No hay evaluación de fuerza activa para este equipo");
                }

                return ResponseContract<ActiveEvaluationDto>.Ok(activeEvaluation, "Evaluación activa encontrada");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<ActiveEvaluationDto>.Fail($"Error al obtener la evaluación activa: {e.Message}");
            }
        }

        /// <summary>
        /// Obtiene información de debugging sobre las evaluaciones de un equipo
        /// </summary>
        /// <param name="teamId">ID del equipo</param>
        /// <returns>Información de debugging</returns>
        public async Task<object> GetEvaluationDebugInfo(int teamId)
        {
            try
            {
                return await _assessStrengthRepository.GetEvaluationDebugInfoAsync(teamId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new { Error = e.Message };
            }
        }
    }
}
