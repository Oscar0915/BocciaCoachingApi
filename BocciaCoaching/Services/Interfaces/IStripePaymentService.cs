using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Payment;

namespace BocciaCoaching.Services.Interfaces
{
    /// <summary>
    /// ES: Interfaz para el servicio de pagos con Stripe
    /// EN: Interface for Stripe payment service
    /// </summary>
    public interface IStripePaymentService
    {
        // Payment Intent methods
        Task<ResponseContract<PaymentIntentResponseDto>> CreatePaymentIntentAsync(CreatePaymentIntentDto createDto);
        Task<ResponseContract<PaymentIntentResponseDto>> ConfirmPaymentIntentAsync(ConfirmPaymentDto confirmDto);
        Task<ResponseContract<PaymentIntentResponseDto>> GetPaymentIntentAsync(string paymentIntentId);
        Task<ResponseContract<bool>> CancelPaymentIntentAsync(string paymentIntentId);

        // Customer methods
        Task<ResponseContract<string>> CreateCustomerAsync(int userId, string email, string? name = null);
        Task<ResponseContract<string>> GetOrCreateCustomerAsync(int userId, string email, string? name = null);
        Task<ResponseContract<bool>> UpdateCustomerAsync(string customerId, string? email = null, string? name = null);

        // Subscription methods (Stripe side)
        Task<ResponseContract<string>> CreateStripeSubscriptionAsync(string customerId, string priceId, string? paymentMethodId = null);
        Task<ResponseContract<bool>> CancelStripeSubscriptionAsync(string subscriptionId, bool atPeriodEnd = true);
        Task<ResponseContract<bool>> UpdateStripeSubscriptionAsync(string subscriptionId, string newPriceId);
        Task<ResponseContract<object>> GetStripeSubscriptionAsync(string subscriptionId);

        // Product and Price methods
        Task<ResponseContract<string>> CreateProductAsync(string name, string description);
        Task<ResponseContract<string>> CreatePriceAsync(string productId, long unitAmount, string currency = "USD", string interval = "month");
        Task<ResponseContract<bool>> UpdateProductAsync(string productId, string? name = null, string? description = null);
        Task<ResponseContract<bool>> DeactivatePriceAsync(string priceId);

        // Webhook methods
        Task<ResponseContract<object>> ConstructWebhookEventAsync(string json, string signature);
        Task<ResponseContract<bool>> ProcessWebhookEventAsync(string eventType, object eventData);

        // Refund methods
        Task<ResponseContract<string>> CreateRefundAsync(string paymentIntentId, long? amount = null);
        Task<ResponseContract<object>> GetRefundAsync(string refundId);

        // Invoice methods
        Task<ResponseContract<string>> CreateInvoiceAsync(string customerId);
        Task<ResponseContract<bool>> FinalizeInvoiceAsync(string invoiceId);
        Task<ResponseContract<object>> GetInvoiceAsync(string invoiceId);

        // Usage and billing
        Task<ResponseContract<bool>> RecordUsageAsync(string subscriptionItemId, int quantity);
        Task<ResponseContract<object>> GetUpcomingInvoiceAsync(string customerId);
    }
}
