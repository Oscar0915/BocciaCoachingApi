using BocciaCoaching.Data;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories
{
    public class LogErrorRepository
    {
        private readonly ApplicationDbContext _context;
        public LogErrorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddLogError(LogError logErroDto)
        {
            try
            {

                var log = new LogError
                {
                  ModuleErrorId = logErroDto.ModuleErrorId,
                  Location = logErroDto.Location,
                  ErrorMessage  = logErroDto.ErrorMessage,
                };

                await _context.LogError.AddAsync(log);
                await _context.SaveChangesAsync();


                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AddUser: {ex.Message}");
                return false;
            }
        }
    }
}
