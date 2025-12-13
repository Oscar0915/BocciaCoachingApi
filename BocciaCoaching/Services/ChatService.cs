using BocciaCoaching.Data;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BocciaCoaching.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ChatService> _logger;

        public ChatService(ApplicationDbContext context, ILogger<ChatService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Conversation>> GetUserConversationsAsync(string userId)
        {
            try
            {
                var conversations = await _context.Set<Conversation>()
                    .Include(c => c.LastMessage)
                    .Include(c => c.Messages)
                    .ToListAsync();

                // Filtrar conversaciones donde el usuario participa
                var userConversations = conversations
                    .Where(c =>
                    {
                        var participants = JsonSerializer.Deserialize<List<string>>(c.Participants);
                        return participants != null && participants.Contains(userId);
                    })
                    .OrderByDescending(c => c.UpdatedAt)
                    .ToList();

                return userConversations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting conversations for user {UserId}", userId);
                throw;
            }
        }

        public async Task<List<Message>> GetMessagesAsync(string conversationId)
        {
            try
            {
                return await _context.Set<Message>()
                    .Where(m => m.ConversationId == conversationId)
                    .OrderBy(m => m.Timestamp)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting messages for conversation {ConversationId}", conversationId);
                throw;
            }
        }

        public async Task<Conversation> CreateConversationAsync(string currentUserId, string participantId)
        {
            try
            {
                // Verificar si ya existe una conversación entre estos usuarios
                var allConversations = await _context.Set<Conversation>().ToListAsync();
                
                var existingConversation = allConversations.FirstOrDefault(c =>
                {
                    var participants = JsonSerializer.Deserialize<List<string>>(c.Participants);
                    return participants != null &&
                           participants.Count == 2 &&
                           participants.Contains(currentUserId) &&
                           participants.Contains(participantId);
                });

                if (existingConversation != null)
                {
                    return existingConversation;
                }

                // Obtener información de los participantes
                var currentUser = await _context.Users.FindAsync(int.Parse(currentUserId));
                var participant = await _context.Users.FindAsync(int.Parse(participantId));

                if (currentUser == null || participant == null)
                {
                    throw new Exception("User not found");
                }

                var participantsList = new List<string> { currentUserId, participantId };
                var participantsDataList = new List<ParticipantData>
                {
                    new ParticipantData
                    {
                        Id = currentUserId,
                        Name = $"{currentUser.FirstName} {currentUser.LastName}",
                        Photo = currentUser.Image ?? string.Empty,
                        Role = "athlete", // Ajustar según tu lógica
                        IsOnline = false
                    },
                    new ParticipantData
                    {
                        Id = participantId,
                        Name = $"{participant.FirstName} {participant.LastName}",
                        Photo = participant.Image ?? string.Empty,
                        Role = "coach", // Ajustar según tu lógica
                        IsOnline = false
                    }
                };

                var conversation = new Conversation
                {
                    Id = Guid.NewGuid().ToString(),
                    Participants = JsonSerializer.Serialize(participantsList),
                    ParticipantsData = JsonSerializer.Serialize(participantsDataList),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Set<Conversation>().Add(conversation);
                await _context.SaveChangesAsync();

                return conversation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating conversation between {UserId1} and {UserId2}", currentUserId, participantId);
                throw;
            }
        }

        public async Task<Message> SaveMessageAsync(Message message)
        {
            try
            {
                // Obtener información del remitente si no está completa
                if (string.IsNullOrEmpty(message.SenderPhoto) && int.TryParse(message.SenderId, out int senderId))
                {
                    var sender = await _context.Users.FindAsync(senderId);
                    if (sender != null)
                    {
                        message.SenderPhoto = sender.Image ?? string.Empty;
                    }
                }

                _context.Set<Message>().Add(message);

                // Actualizar la conversación
                var conversation = await _context.Set<Conversation>()
                    .FirstOrDefaultAsync(c => c.Id == message.ConversationId);

                if (conversation != null)
                {
                    conversation.UpdatedAt = DateTime.UtcNow;
                    conversation.LastMessageId = message.Id;
                    conversation.UnreadCount++;
                }

                await _context.SaveChangesAsync();

                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving message");
                throw;
            }
        }

        public async Task MarkAsReadAsync(string messageId)
        {
            try
            {
                var message = await _context.Set<Message>().FindAsync(messageId);
                if (message != null)
                {
                    message.IsRead = true;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking message {MessageId} as read", messageId);
                throw;
            }
        }

        public async Task MarkConversationAsReadAsync(string conversationId, string userId)
        {
            try
            {
                var messages = await _context.Set<Message>()
                    .Where(m => m.ConversationId == conversationId && m.SenderId != userId && !m.IsRead)
                    .ToListAsync();

                foreach (var message in messages)
                {
                    message.IsRead = true;
                }

                var conversation = await _context.Set<Conversation>()
                    .FirstOrDefaultAsync(c => c.Id == conversationId);

                if (conversation != null)
                {
                    conversation.UnreadCount = 0;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking conversation {ConversationId} as read", conversationId);
                throw;
            }
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            try
            {
                var conversations = await _context.Set<Conversation>().ToListAsync();

                var userConversations = conversations
                    .Where(c =>
                    {
                        var participants = JsonSerializer.Deserialize<List<string>>(c.Participants);
                        return participants != null && participants.Contains(userId);
                    })
                    .ToList();

                var unreadCount = 0;
                foreach (var conversation in userConversations)
                {
                    var unreadMessages = await _context.Set<Message>()
                        .CountAsync(m => m.ConversationId == conversation.Id &&
                                       m.SenderId != userId &&
                                       !m.IsRead);
                    unreadCount += unreadMessages;
                }

                return unreadCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread count for user {UserId}", userId);
                throw;
            }
        }

        public async Task<List<Conversation>> SearchConversationsAsync(string userId, string query)
        {
            try
            {
                var conversations = await GetUserConversationsAsync(userId);

                if (string.IsNullOrWhiteSpace(query))
                {
                    return conversations;
                }

                return conversations
                    .Where(c =>
                    {
                        var participantsData = JsonSerializer.Deserialize<List<ParticipantData>>(c.ParticipantsData);
                        return participantsData != null &&
                               participantsData.Any(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching conversations for user {UserId}", userId);
                throw;
            }
        }

        public async Task DeleteConversationAsync(string conversationId)
        {
            try
            {
                var conversation = await _context.Set<Conversation>()
                    .Include(c => c.Messages)
                    .FirstOrDefaultAsync(c => c.Id == conversationId);

                if (conversation != null)
                {
                    _context.Set<Message>().RemoveRange(conversation.Messages);
                    _context.Set<Conversation>().Remove(conversation);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting conversation {ConversationId}", conversationId);
                throw;
            }
        }

        public async Task UpdateUserOnlineStatus(string userId, bool isOnline)
        {
            try
            {
                var conversations = await _context.Set<Conversation>().ToListAsync();

                foreach (var conversation in conversations)
                {
                    var participantsData = JsonSerializer.Deserialize<List<ParticipantData>>(conversation.ParticipantsData);
                    if (participantsData != null)
                    {
                        var participant = participantsData.FirstOrDefault(p => p.Id == userId);
                        if (participant != null)
                        {
                            participant.IsOnline = isOnline;
                            conversation.ParticipantsData = JsonSerializer.Serialize(participantsData);
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating online status for user {UserId}", userId);
                throw;
            }
        }
    }
}

