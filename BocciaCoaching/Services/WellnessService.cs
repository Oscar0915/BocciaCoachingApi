using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Wellness;
using BocciaCoaching.Repositories.Interfaces.IWellness;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class WellnessService : IWellnessService
    {
        private readonly IWellnessRepository _repository;

        public WellnessService(IWellnessRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseContract<ResponseDailyWellnessDto>> RegisterDailyWellness(AddDailyWellnessDto dto)
        {
            try
            {
                if (!AreValuesInRange(dto.Sleep, dto.Stress, dto.Fatigue, dto.MusclePain))
                    return ResponseContract<ResponseDailyWellnessDto>.Fail("Todos los indicadores deben estar entre 1 y 7.");

                return await _repository.CreateIfNotExistsForDayAsync(dto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<ResponseDailyWellnessDto>.Fail($"Error al registrar el test de bienestar: {e.Message}");
            }
        }

        public async Task<ResponseContract<ResponseDailyWellnessDto>> UpdateDailyWellness(UpdateDailyWellnessDto dto)
        {
            try
            {
                if (!AreValuesInRange(dto.Sleep, dto.Stress, dto.Fatigue, dto.MusclePain))
                    return ResponseContract<ResponseDailyWellnessDto>.Fail("Todos los indicadores deben estar entre 1 y 7.");

                return await _repository.UpdateAsync(dto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<ResponseDailyWellnessDto>.Fail($"Error al actualizar el test de bienestar: {e.Message}");
            }
        }

        public async Task<ResponseContract<ResponseDailyWellnessDto>> GetTodayWellness(Guid athleteId)
        {
            return await GetWellnessByDate(athleteId, DateTime.Now);
        }

        public async Task<ResponseContract<ResponseDailyWellnessDto>> GetWellnessByDate(Guid athleteId, DateTime date)
        {
            try
            {
                var result = await _repository.GetByAthleteAndDateAsync(athleteId, date);
                if (result == null)
                    return ResponseContract<ResponseDailyWellnessDto>.Fail("El atleta no ha registrado su test de bienestar para esta fecha.");

                return ResponseContract<ResponseDailyWellnessDto>.Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<ResponseDailyWellnessDto>.Fail($"Error al obtener el test de bienestar: {e.Message}");
            }
        }

        public async Task<ResponseContract<ResponseDailyWellnessDto>> GetWellnessById(Guid dailyWellnessId)
        {
            try
            {
                var result = await _repository.GetByIdAsync(dailyWellnessId);
                if (result == null)
                    return ResponseContract<ResponseDailyWellnessDto>.Fail("No se encontró el test de bienestar indicado.");

                return ResponseContract<ResponseDailyWellnessDto>.Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<ResponseDailyWellnessDto>.Fail($"Error al obtener el test de bienestar: {e.Message}");
            }
        }

        public async Task<ResponseContract<WellnessAthleteHistoryDto>> GetAthleteHistory(Guid athleteId)
        {
            try
            {
                var result = await _repository.GetAthleteHistoryAsync(athleteId);
                if (result == null)
                    return ResponseContract<WellnessAthleteHistoryDto>.Fail("No se encontró el atleta indicado.");

                return ResponseContract<WellnessAthleteHistoryDto>.Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<WellnessAthleteHistoryDto>.Fail($"Error al obtener el historial de bienestar: {e.Message}");
            }
        }

        public async Task<ResponseContract<List<ResponseDailyWellnessDto>>> GetTeamWellnessByDate(Guid teamId, DateTime date)
        {
            try
            {
                var result = await _repository.GetTeamWellnessByDateAsync(teamId, date);
                return ResponseContract<List<ResponseDailyWellnessDto>>.Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<ResponseDailyWellnessDto>>.Fail($"Error al obtener los tests de bienestar del equipo: {e.Message}");
            }
        }

        private static bool AreValuesInRange(params int[] values)
        {
            return values.All(v => v >= 1 && v <= 7);
        }
    }
}

