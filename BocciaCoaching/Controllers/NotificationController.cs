using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Notification;
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
        public async Task<ActionResult<ResponseContract<IEnumerable<NotificationTypeDto>>>> GetTypes()
        {
            var result = await _notificationService.GetAllTypes();
            return Ok(result);
        }

        [HttpGet("GetType/{id}")]
        public async Task<ActionResult<ResponseContract<NotificationTypeDto>>> GetTypeById(int id)
        {
            var result = await _notificationService.GetTypeById(id);
            return Ok(result);
        }

        [HttpPost("CreateType")]
        public async Task<ActionResult<ResponseContract<bool>>> CreateType([FromBody] RequestCreateNotificationTypeDto type)
        {
            var result = await _notificationService.CreateType(type);
            return Ok(result);
        }

        [HttpPut("UpdateType")]
        public async Task<ActionResult<ResponseContract<bool>>> UpdateType([FromBody] RequestUpdateNotificationTypeDto type)
        {
            var result = await _notificationService.UpdateType(type);
            return Ok(result);
        }

        [HttpGet("GetMessage/{id}")]
        public async Task<ActionResult<ResponseContract<NotificationMessageDto>>> GetMessageById(int id)
        {
            var result = await _notificationService.GetMessageById(id);
            return Ok(result);
        }

        [HttpPost("CreateMessage")]
        public async Task<ActionResult<ResponseContract<bool>>> CreateMessage([FromBody] RequestCreateNotificationMessageDto message)
        {
            var result = await _notificationService.CreateMessage(message);
            return Ok(result);
        }

        [HttpPut("UpdateMessage")]
        public async Task<ActionResult<ResponseContract<bool>>> UpdateMessage([FromBody] RequestUpdateNotificationMessageDto message)
        {
            var result = await _notificationService.UpdateMessage(message);
            return Ok(result);
        }

        [HttpGet("GetMessagesByCoach/{coachId}")]
        public async Task<ActionResult<ResponseContract<IEnumerable<NotificationMessageDto>>>> GetMessagesByCoach(int coachId, [FromQuery] int? page = null, [FromQuery] int? pageSize = null)
        {
            var result = await _notificationService.GetMessagesByCoach(coachId, page, pageSize);
            return Ok(result);
        }

        [HttpGet("GetMessagesByAthlete/{athleteId}")]
        public async Task<ActionResult<ResponseContract<IEnumerable<NotificationMessageDto>>>> GetMessagesByAthlete(int athleteId, [FromQuery] int? page = null, [FromQuery] int? pageSize = null)
        {
            var result = await _notificationService.GetMessagesByAthlete(athleteId, page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Enviar invitación a un atleta para unirse a un equipo
        /// </summary>
        [HttpPost("SendTeamInvitation")]
        public async Task<ActionResult<ResponseContract<bool>>> SendTeamInvitation([FromBody] SendTeamInvitationDto dto)
        {
            var result = await _notificationService.SendTeamInvitation(dto.CoachId, dto.Email, dto.TeamId, dto.Message);
            return Ok(result);
        }

        /// <summary>
        /// Aceptar invitación de equipo
        /// </summary>
        [HttpPut("AcceptTeamInvitation/{notificationMessageId}")]
        public async Task<ActionResult<ResponseContract<bool>>> AcceptTeamInvitation(int notificationMessageId)
        {
            var result = await _notificationService.AcceptTeamInvitation(notificationMessageId);
            return Ok(result);
        }
    }
}
