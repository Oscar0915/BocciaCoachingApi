using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Notification;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("GetTypes")]
        public async Task<ActionResult<ResponseContract<IEnumerable<NotificationType>?>?>> GetTypes()
        {
            var result = await _notificationService.GetAllTypes();
            return Ok(result);
        }

        [HttpGet("GetType/{id}")]
        public async Task<ActionResult<ResponseContract<NotificationType?>?>> GetTypeById(int id)
        {
            var result = await _notificationService.GetTypeById(id);
            return Ok(result);
        }

        [HttpPost("CreateType")]
        public async Task<ActionResult<ResponseContract<bool?>?>> CreateType([FromBody] RequestCreateNotificationTypeDto type)
        {
            var result = await _notificationService.CreateType(type);
            return Ok(result);
        }

        [HttpPut("UpdateType")]
        public async Task<ActionResult<ResponseContract<bool?>?>> UpdateType([FromBody] RequestUpdateNotificationTypeDto type)
        {
            var result = await _notificationService.UpdateType(type);
            return Ok(result);
        }

        [HttpGet("GetMessage/{id}")]
        public async Task<ActionResult<ResponseContract<NotificationMessage?>?>> GetMessageById(int id)
        {
            var result = await _notificationService.GetMessageById(id);
            return Ok(result);
        }

        [HttpPost("CreateMessage")]
        public async Task<ActionResult<ResponseContract<bool?>?>> CreateMessage([FromBody] RequestCreateNotificationMessageDto message)
        {
            var result = await _notificationService.CreateMessage(message);
            return Ok(result);
        }

        [HttpPut("UpdateMessage")]
        public async Task<ActionResult<ResponseContract<bool?>?>> UpdateMessage([FromBody] RequestUpdateNotificationMessageDto message)
        {
            var result = await _notificationService.UpdateMessage(message);
            return Ok(result);
        }

        [HttpGet("GetMessagesByCoach/{coachId}")]
        public async Task<ActionResult<ResponseContract<IEnumerable<NotificationMessage>?>?>> GetMessagesByCoach(int coachId, [FromQuery] int? page = null, [FromQuery] int? pageSize = null, [FromQuery] bool? status = null)
        {
            var result = await _notificationService.GetMessagesByCoach(coachId, page, pageSize, status);
            return Ok(result);
        }

        [HttpGet("GetMessagesByAthlete/{athleteId}")]
        public async Task<ActionResult<ResponseContract<IEnumerable<NotificationMessage>?>?>> GetMessagesByAthlete(int athleteId, [FromQuery] int? page = null, [FromQuery] int? pageSize = null, [FromQuery] bool? status = null)
        {
            var result = await _notificationService.GetMessagesByAthlete(athleteId, page, pageSize, status);
            return Ok(result);
        }
    }
}
