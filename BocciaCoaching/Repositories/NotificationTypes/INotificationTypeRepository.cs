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
        Task<NotificationType?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(NotificationType? notificationType);

        // Métodos para NotificationMessage
        Task<bool> AddMessageAsync(NotificationMessage? message);
        Task<bool> UpdateMessageAsync(NotificationMessage? message);
        Task<NotificationMessage?> GetMessageByIdAsync(int id);

        // Nuevos métodos
        Task<IEnumerable<NotificationMessage>> GetMessagesByCoachAsync(int coachId);
        Task<IEnumerable<NotificationMessage>> GetMessagesByAthleteAsync(int athleteId);
    }
}
