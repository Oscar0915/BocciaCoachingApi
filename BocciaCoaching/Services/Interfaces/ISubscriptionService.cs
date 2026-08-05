using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Subscription;
using BocciaCoaching.Models.DTO.Payment;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Services.Interfaces
{
    /// <summary>
    /// ES: Interfaz para el servicio de suscripciones
    /// EN: Interface for subscription service
    /// </summary>
    public interface ISubscriptionService
    {
        // Subscription Type methods
        Task<ResponseContract<IEnumerable<SubscriptionTypeDto>>> GetAllSubscriptionTypesAsync();
        Task<ResponseContract<SubscriptionTypeDto>> GetSubscriptionTypeByIdAsync(Guid id);
        Task<ResponseContract<SubscriptionTypeDto>> CreateSubscriptionTypeAsync(CreateSubscriptionTypeDto createDto);
        Task<ResponseContract<SubscriptionTypeDto>> UpdateSubscriptionTypeAsync(Guid id, CreateSubscriptionTypeDto updateDto);
        Task<ResponseContract<bool>> DeleteSubscriptionTypeAsync(Guid id);

        // User Subscription methods
        Task<ResponseContract<UserSubscriptionDto>> GetUserSubscriptionAsync(Guid userId);
        Task<ResponseContract<IEnumerable<UserSubscriptionDto>>> GetUserSubscriptionHistoryAsync(Guid userId);
        Task<ResponseContract<SubscriptionCreateResponseDto>> CreateSubscriptionAsync(CreateSubscriptionDto createDto);
        Task<ResponseContract<bool>> CancelSubscriptionAsync(CancelSubscriptionDto cancelDto);
        Task<ResponseContract<UserSubscriptionDto>> UpdateSubscriptionAsync(UpdateSubscriptionDto updateDto);
        Task<ResponseContract<UserSubscriptionDto>> ReactivateSubscriptionAsync(Guid subscriptionId);

        // Trial methods
        Task<ResponseContract<UserSubscriptionDto>> StartTrialAsync(Guid userId, Guid subscriptionTypeId, int trialDays = 7);
        Task<ResponseContract<bool>> IsTrialAvailableAsync(Guid userId, Guid subscriptionTypeId);

        // Subscription validation
        Task<ResponseContract<bool>> ValidateUserSubscriptionAsync(Guid userId, string feature);
        Task<ResponseContract<bool>> HasActiveSubscriptionAsync(Guid userId);
        Task<ResponseContract<bool>> CanAccessFeatureAsync(Guid userId, string featureName);

        // Subscription limits
        Task<ResponseContract<bool>> CanCreateTeamAsync(Guid userId);
        Task<ResponseContract<bool>> CanAddAthleteAsync(Guid userId, Guid teamId);
        Task<ResponseContract<bool>> CanPerformEvaluationAsync(Guid userId);
        Task<ResponseContract<int>> GetRemainingTeamsAsync(Guid userId);
        Task<ResponseContract<int>> GetRemainingAthletesAsync(Guid userId, Guid teamId);
        Task<ResponseContract<int>> GetRemainingEvaluationsAsync(Guid userId);

        // Webhook handling
        Task<ResponseContract<bool>> HandleStripeWebhookAsync(string eventType, object eventData);
        Task<ResponseContract<bool>> ProcessSubscriptionUpdatedAsync(string stripeSubscriptionId);
        Task<ResponseContract<bool>> ProcessPaymentSucceededAsync(string stripePaymentIntentId);
        Task<ResponseContract<bool>> ProcessPaymentFailedAsync(string stripePaymentIntentId);
    }
}
