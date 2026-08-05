using BocciaCoaching.Data;
using BocciaCoaching.Repositories.Interfaces.IRol;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.Rol
{
    public class RolRepository : IRolRepository
    {
        private readonly ApplicationDbContext _context;

        public RolRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Entities.Rol> CreateAsync(Models.Entities.Rol rol)
        {
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task<List<Models.Entities.Rol>> GetAllAsync()
        {
            return await _context.Roles
                .OrderBy(r => r.Description)
                .ToListAsync();
        }

        public async Task<Models.Entities.Rol?> GetByIdAsync(Guid id)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RolId == id);
        }

        public async Task<bool> UpdateAsync(Models.Entities.Rol rol)
        {
            _context.Roles.Update(rol);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Roles.FindAsync(id);
            if (entity == null) return false;
            _context.Roles.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByDescriptionAsync(string description, Guid? excludeId = null)
        {
            return await _context.Roles
                .AnyAsync(r => r.Description.ToLower() == description.ToLower()
                            && (excludeId == null || r.RolId != excludeId));
        }

        public async Task<bool> IsAssignedToUsersAsync(Guid id)
        {
            return await _context.UserRoles.AnyAsync(ur => ur.RolId == id);
        }
    }
}

