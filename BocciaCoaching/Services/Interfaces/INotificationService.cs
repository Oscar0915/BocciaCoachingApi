using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Notification;

namespace BocciaCoaching.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ResponseContract<IEnumerable<NotificationTypeDto>>> GetAllTypes();
        Task<ResponseContract<NotificationTypeDto>> GetTypeById(int id);
        Task<ResponseContract<bool>> CreateType(RequestCreateNotificationTypeDto? type);
        Task<ResponseContract<bool>> UpdateType(RequestUpdateNotificationTypeDto? type);

        Task<ResponseContract<NotificationMessageDto>> GetMessageById(int id);
        Task<ResponseContract<bool>> CreateMessage(RequestCreateNotificationMessageDto? message);
        Task<ResponseContract<bool>> UpdateMessage(RequestUpdateNotificationMessageDto? message);

        // Listados con paginación/filtro
        Task<ResponseContract<IEnumerable<NotificationMessageDto>>> GetMessagesByCoach(int coachId, int? page = null, int? pageSize = null, bool? status = null);
        Task<ResponseContract<IEnumerable<NotificationMessageDto>>> GetMessagesByAthlete(int athleteId, int? page = null, int? pageSize = null, bool? status = null);
    }
}
