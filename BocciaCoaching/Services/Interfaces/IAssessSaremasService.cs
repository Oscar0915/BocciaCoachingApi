using BocciaCoaching.Models.DTO.AssessSaremas;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IAssessSaremasService
    {
        Task<ResponseContract<ResponseAddSaremasDto>> CreateEvaluation(AddSaremasEvaluationDto dto);
        Task<ResponseContract<SaremasAthleteEvaluation>> AddAthleteToEvaluation(RequestAddAthleteToSaremasDto dto);
        Task<ResponseContract<bool>> AddThrowDetail(RequestAddSaremasDetailDto dto);
        Task<ResponseContract<ActiveSaremasEvaluationDto>> GetActiveEvaluation(Guid teamId, Guid coachId);
        Task<ResponseContract<bool>> UpdateState(UpdateSaremasStateDto dto);
        Task<ResponseContract<bool>> CancelEvaluation(CancelSaremasDto dto);
        Task<ResponseContract<List<SaremasEvaluationSummaryDto>>> GetTeamEvaluations(Guid teamId);
        Task<ResponseContract<SaremasEvaluationDetailsDto>> GetEvaluationDetails(Guid saremasEvalId);
        Task<ResponseContract<SaremasStatisticsDto>> GetEvaluationStatistics(Guid saremasEvalId);
        Task<ResponseContract<SaremasAthleteHistoryDto>> GetAthleteHistory(Guid athleteId);
        Task<ResponseContract<CoachHasSaremasEvaluationsDto>> CoachHasEvaluations(Guid coachId);
    }
}

