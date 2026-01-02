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
        Task<ResponseContract<SubscriptionTypeDto>> GetSubscriptionTypeByIdAsync(int id);
        Task<ResponseContract<SubscriptionTypeDto>> CreateSubscriptionTypeAsync(CreateSubscriptionTypeDto createDto);
        Task<ResponseContract<SubscriptionTypeDto>> UpdateSubscriptionTypeAsync(int id, CreateSubscriptionTypeDto updateDto);
        Task<ResponseContract<bool>> DeleteSubscriptionTypeAsync(int id);

        // User Subscription methods
        Task<ResponseContract<UserSubscriptionDto>> GetUserSubscriptionAsync(int userId);
        Task<ResponseContract<IEnumerable<UserSubscriptionDto>>> GetUserSubscriptionHistoryAsync(int userId);
        Task<ResponseContract<SubscriptionCreateResponseDto>> CreateSubscriptionAsync(CreateSubscriptionDto createDto);
        Task<ResponseContract<bool>> CancelSubscriptionAsync(CancelSubscriptionDto cancelDto);
        Task<ResponseContract<UserSubscriptionDto>> UpdateSubscriptionAsync(UpdateSubscriptionDto updateDto);
        Task<ResponseContract<UserSubscriptionDto>> ReactivateSubscriptionAsync(int subscriptionId);

        // Trial methods
        Task<ResponseContract<UserSubscriptionDto>> StartTrialAsync(int userId, int subscriptionTypeId, int trialDays = 7);
        Task<ResponseContract<bool>> IsTrialAvailableAsync(int userId, int subscriptionTypeId);

        // Subscription validation
        Task<ResponseContract<bool>> ValidateUserSubscriptionAsync(int userId, string feature);
        Task<ResponseContract<bool>> HasActiveSubscriptionAsync(int userId);
        Task<ResponseContract<bool>> CanAccessFeatureAsync(int userId, string featureName);

        // Subscription limits
        Task<ResponseContract<bool>> CanCreateTeamAsync(int userId);
        Task<ResponseContract<bool>> CanAddAthleteAsync(int userId, int teamId);
        Task<ResponseContract<bool>> CanPerformEvaluationAsync(int userId);
        Task<ResponseContract<int>> GetRemainingTeamsAsync(int userId);
        Task<ResponseContract<int>> GetRemainingAthletesAsync(int userId, int teamId);
        Task<ResponseContract<int>> GetRemainingEvaluationsAsync(int userId);

        // Webhook handling
        Task<ResponseContract<bool>> HandleStripeWebhookAsync(string eventType, object eventData);
        Task<ResponseContract<bool>> ProcessSubscriptionUpdatedAsync(string stripeSubscriptionId);
        Task<ResponseContract<bool>> ProcessPaymentSucceededAsync(string stripePaymentIntentId);
        Task<ResponseContract<bool>> ProcessPaymentFailedAsync(string stripePaymentIntentId);
    }
}
