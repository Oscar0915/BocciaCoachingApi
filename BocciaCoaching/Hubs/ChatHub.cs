using BocciaCoaching.Models.Entities;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace BocciaCoaching.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IChatService chatService, ILogger<ChatHub> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        // Unirse a una conversación
        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);

            _logger.LogInformation($"User {Context.ConnectionId} joined conversation {conversationId}");

            // Notificar a otros usuarios
            await Clients.OthersInGroup(conversationId)
                .SendAsync("UserConnected", Context.ConnectionId);
        }

        // Salir de una conversación
        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);

            _logger.LogInformation($"User {Context.ConnectionId} left conversation {conversationId}");
        }

        // Enviar mensaje
        public async Task SendMessage(string conversationId, string senderId, string senderName, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(senderId))
                {
                    _logger.LogWarning("Sender ID not provided");
                    return;
                }

                // Guardar mensaje en la base de datos
                var message = await _chatService.SaveMessageAsync(new Message
                {
                    ConversationId = conversationId,
                    SenderId = senderId,
                    SenderName = senderName,
                    Text = text,
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                });

                // Enviar mensaje a todos en el grupo
                await Clients.Group(conversationId).SendAsync("ReceiveMessage", new
                {
                    id = message.Id,
                    conversationId = message.ConversationId,
                    senderId = message.SenderId,
                    senderName = message.SenderName,
                    senderPhoto = message.SenderPhoto,
                    text = message.Text,
                    timestamp = message.Timestamp,
                    isRead = message.IsRead
                });

                _logger.LogInformation($"Message sent in conversation {conversationId} by {senderName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
                await Clients.Caller.SendAsync("Error", "Failed to send message");
            }
        }

        // Indicador de escritura
        public async Task Typing(string conversationId, string userName)
        {
            await Clients.OthersInGroup(conversationId)
                .SendAsync("UserTyping", userName);
        }

        // Marcar mensaje como leído
        public async Task MarkAsRead(string messageId)
        {
            try
            {
                await _chatService.MarkAsReadAsync(messageId);
                await Clients.Caller.SendAsync("MessageRead", messageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking message as read");
            }
        }

        // Conexión establecida
        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"User connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        // Desconexión
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"User disconnected: {Context.ConnectionId}");
            
            if (exception != null)
            {
                _logger.LogError(exception, "User disconnected with error");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}

