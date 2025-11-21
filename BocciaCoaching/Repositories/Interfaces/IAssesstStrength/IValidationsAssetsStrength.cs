using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.IAssesstStrength;

public interface IValidationsAssetsStrength
{
    Task<bool> IsUpdateDetailAssessStrength(
        RequestAddDetailToEvaluationForAthlete requestAddDetailToEvaluationForAthlete);

    Task<bool> IsActiveTeam(Team team);

}