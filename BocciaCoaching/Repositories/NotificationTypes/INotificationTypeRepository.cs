using BocciaCoaching.Models.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BocciaCoaching.Repositories.NotificationTypes
{
    public interface INotificationTypeRepository
    {
        Task<bool> AddAsync(NotificationType? notificationType);

        // Nuevos métodos para NotificationType
        Task<IEnumerable<NotificationType>> GetAllAsync();
        Task<NotificationType?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(NotificationType? notificationType);

        // Métodos para NotificationMessage
        Task<bool> AddMessageAsync(NotificationMessage? message);
        Task<bool> UpdateMessageAsync(NotificationMessage? message);
        Task<NotificationMessage?> GetMessageByIdAsync(Guid id);

        // Nuevos métodos
        Task<IEnumerable<NotificationMessage>> GetMessagesByCoachAsync(Guid coachId);
        Task<IEnumerable<NotificationMessage>> GetMessagesByAthleteAsync(Guid athleteId);
    }
}
