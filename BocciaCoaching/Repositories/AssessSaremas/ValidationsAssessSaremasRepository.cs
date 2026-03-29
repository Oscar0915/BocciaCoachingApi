using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.AssessSaremas;
using BocciaCoaching.Repositories.Interfaces.IAssessSaremas;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.AssessSaremas
{
    public class ValidationsAssessSaremasRepository : IValidationsAssessSaremas
    {
        private readonly ApplicationDbContext _context;

        public ValidationsAssessSaremasRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsThrowDuplicateAsync(RequestAddSaremasDetailDto dto)
        {
            return await _context.SaremasThrows.AnyAsync(t =>
                t.SaremasEvalId == dto.SaremasEvalId &&
                t.AthleteId == dto.AthleteId &&
                t.ThrowNumber == dto.ThrowNumber);
        }
    }
}

