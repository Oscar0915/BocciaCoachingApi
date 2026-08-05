using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Notification;

namespace BocciaCoaching.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ResponseContract<IEnumerable<NotificationTypeDto>>> GetAllTypes();
        Task<ResponseContract<NotificationTypeDto>> GetTypeById(Guid id);
        Task<ResponseContract<bool>> CreateType(RequestCreateNotificationTypeDto? type);
        Task<ResponseContract<bool>> UpdateType(RequestUpdateNotificationTypeDto? type);

        Task<ResponseContract<NotificationMessageDto>> GetMessageById(Guid id);
        Task<ResponseContract<bool>> CreateMessage(RequestCreateNotificationMessageDto? message);
        Task<ResponseContract<bool>> UpdateMessage(RequestUpdateNotificationMessageDto? message);

        // Listados con paginación/filtro
        Task<ResponseContract<IEnumerable<NotificationMessageDto>>> GetMessagesByCoach(Guid coachId, int? page = null, int? pageSize = null);
        Task<ResponseContract<IEnumerable<NotificationMessageDto>>> GetMessagesByAthlete(Guid athleteId, int? page = null, int? pageSize = null);
        
        // Invitaciones de equipo
        Task<ResponseContract<bool>> SendTeamInvitation(Guid coachId, string athleteEmail, Guid teamId, string? message = null);
        Task<ResponseContract<bool>> AcceptTeamInvitation(Guid notificationMessageId);
    }
}
