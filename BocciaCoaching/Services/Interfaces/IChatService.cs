using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IChatService
    {
        Task<List<Conversation>> GetUserConversationsAsync(string userId);
        Task<List<Message>> GetMessagesAsync(string conversationId);
        Task<Conversation> CreateConversationAsync(string currentUserId, string participantId);
        Task<Message> SaveMessageAsync(Message message);
        Task MarkAsReadAsync(string messageId);
        Task MarkConversationAsReadAsync(string conversationId, string userId);
        Task<int> GetUnreadCountAsync(string userId);
        Task<List<Conversation>> SearchConversationsAsync(string userId, string query);
        Task DeleteConversationAsync(string conversationId);
        Task UpdateUserOnlineStatus(string userId, bool isOnline);
    }
}

