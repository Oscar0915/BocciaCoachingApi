using BocciaCoaching.Models.DTO.AssessDirection;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.IAssessDirection;

public interface IValidationsAssetsDirection
{
    Task<bool> IsUpdateDetailAssessDirection(
        RequestAddDetailToDirectionEvaluation requestAddDetailToDirectionEvaluation);

    Task<bool> IsActiveTeam(Team team);
}

