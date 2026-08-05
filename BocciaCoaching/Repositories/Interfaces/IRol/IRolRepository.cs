using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.IRol
{
    public interface IRolRepository
    {
        Task<Models.Entities.Rol> CreateAsync(Models.Entities.Rol rol);
        Task<List<Models.Entities.Rol>> GetAllAsync();
        Task<Models.Entities.Rol?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Models.Entities.Rol rol);
        Task<bool> DeleteAsync(Guid id);

        /// <summary>ES: Indica si existe un rol con la misma descripción (opcionalmente excluyendo un Id).</summary>
        Task<bool> ExistsByDescriptionAsync(string description, Guid? excludeId = null);

        /// <summary>ES: Indica si el rol está asignado a algún usuario (UserRol).</summary>
        Task<bool> IsAssignedToUsersAsync(Guid id);
    }
}

