namespace BocciaCoaching.Models.DTO
{
    public class CreateConversationRequest
    {
        public string? CurrentUserId { get; set; }
        public string ParticipantId { get; set; } = string.Empty;
    }

    public class SendMessageRequest
    {
        public string ConversationId { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}

