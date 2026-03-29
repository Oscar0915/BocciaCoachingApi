using BocciaCoaching.Models.DTO.AssessSaremas;

namespace BocciaCoaching.Repositories.Interfaces.IAssessSaremas
{
    public interface IValidationsAssessSaremas
    {
        Task<bool> IsThrowDuplicateAsync(RequestAddSaremasDetailDto dto);
    }
}

