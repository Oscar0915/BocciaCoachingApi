namespace BocciaCoaching.Models.DTO.Payment
{
    /// <summary>
    /// ES: DTO para información de pago
    /// EN: DTO for payment information
    /// </summary>
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int SubscriptionId { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? PaymentMethod { get; set; }
        public string? Description { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? ReceiptNumber { get; set; }
    }

    /// <summary>
    /// ES: DTO para crear intención de pago
    /// EN: DTO for creating payment intent
    /// </summary>
    public class CreatePaymentIntentDto
    {
        public int SubscriptionTypeId { get; set; }
        public int UserId { get; set; }
        public bool IsAnnual { get; set; } = false;
        public string Currency { get; set; } = "USD";
        public string? PaymentMethodId { get; set; }
        public bool ConfirmPayment { get; set; } = false;
    }

    /// <summary>
    /// ES: DTO para respuesta de intención de pago
    /// EN: DTO for payment intent response
    /// </summary>
    public class PaymentIntentResponseDto
    {
        public bool Success { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string? Message { get; set; }
        public bool RequiresAction { get; set; } = false;
    }

    /// <summary>
    /// ES: DTO para confirmar pago
    /// EN: DTO for confirming payment
    /// </summary>
    public class ConfirmPaymentDto
    {
        public string PaymentIntentId { get; set; } = string.Empty;
        public string? PaymentMethodId { get; set; }
    }

    /// <summary>
    /// ES: DTO para webhook de Stripe
    /// EN: DTO for Stripe webhook
    /// </summary>
    public class StripeWebhookDto
    {
        public string EventType { get; set; } = string.Empty;
        public object Data { get; set; } = new();
        public string StripeSignature { get; set; } = string.Empty;
    }
}
