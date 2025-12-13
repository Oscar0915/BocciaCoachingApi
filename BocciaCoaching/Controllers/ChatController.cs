using BocciaCoaching.Models.DTO;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        // GET: api/chat/conversations?userId=123
        [HttpGet("conversations")]
        public async Task<ActionResult<List<Conversation>>> GetUserConversations([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("User ID is required");

                var conversations = await _chatService.GetUserConversationsAsync(userId);
                return Ok(conversations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting conversations for user {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/chat/conversations/{id}/messages
        [HttpGet("conversations/{id}/messages")]
        public async Task<ActionResult<List<Message>>> GetConversationMessages(string id)
        {
            try
            {
                var messages = await _chatService.GetMessagesAsync(id);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting messages for conversation {ConversationId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/chat/conversations
        [HttpPost("conversations")]
        public async Task<ActionResult<Conversation>> CreateConversation(
            [FromBody] CreateConversationRequest request)
        {
            try
            {
                // En este caso, asumimos que currentUserId viene en el header o query string
                var currentUserId = Request.Headers["X-User-Id"].ToString();
                
                if (string.IsNullOrEmpty(currentUserId))
                {
                    currentUserId = Request.Query["currentUserId"].ToString();
                }

                if (string.IsNullOrEmpty(currentUserId))
                    return BadRequest("Current user ID is required");

                if (string.IsNullOrEmpty(request.ParticipantId))
                    return BadRequest("Participant ID is required");

                var conversation = await _chatService.CreateConversationAsync(
                    currentUserId,
                    request.ParticipantId
                );

                return CreatedAtAction(
                    nameof(GetConversationMessages),
                    new { id = conversation.Id },
                    conversation
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating conversation");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/chat/conversations/{id}/read
        [HttpPut("conversations/{id}/read")]
        public async Task<IActionResult> MarkConversationAsRead(string id, [FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("User ID is required");

                await _chatService.MarkConversationAsReadAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking conversation as read");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/chat/unread-count?userId=123
        [HttpGet("unread-count")]
        public async Task<ActionResult<object>> GetUnreadCount([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("User ID is required");

                var count = await _chatService.GetUnreadCountAsync(userId);
                return Ok(new { count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread count");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/chat/conversations/search?userId=123&query=john
        [HttpGet("conversations/search")]
        public async Task<ActionResult<List<Conversation>>> SearchConversations(
            [FromQuery] string userId,
            [FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("User ID is required");

                var conversations = await _chatService.SearchConversationsAsync(userId, query);
                return Ok(conversations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching conversations");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/chat/conversations/{id}
        [HttpDelete("conversations/{id}")]
        public async Task<IActionResult> DeleteConversation(string id)
        {
            try
            {
                await _chatService.DeleteConversationAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting conversation");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

