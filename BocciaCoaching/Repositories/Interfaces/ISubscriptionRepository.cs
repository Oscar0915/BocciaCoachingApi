using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces
{
    /// <summary>
    /// ES: Interfaz para el repositorio de suscripciones
    /// EN: Interface for subscription repository
    /// </summary>
    public interface ISubscriptionRepository
    {
        // Subscription methods
        Task<IEnumerable<Subscription>> GetAllAsync();
        Task<Subscription?> GetByIdAsync(Guid id);
        Task<Subscription?> GetByUserIdAsync(Guid userId);
        Task<Subscription?> GetActiveByUserIdAsync(Guid userId);
        Task<IEnumerable<Subscription>> GetByUserIdAllAsync(Guid userId);
        Task<Subscription?> GetByStripeSubscriptionIdAsync(string stripeSubscriptionId);
        Task<Subscription> AddAsync(Subscription subscription);
        Task<Subscription> UpdateAsync(Subscription subscription);
        Task DeleteAsync(Guid id);
        Task<bool> HasActiveSubscriptionAsync(Guid userId);
        Task<IEnumerable<Subscription>> GetExpiringSubscriptionsAsync(DateTime date);

        // SubscriptionType methods
        Task<IEnumerable<SubscriptionType>> GetAllSubscriptionTypesAsync();
        Task<SubscriptionType?> GetSubscriptionTypeByIdAsync(Guid id);
        Task<SubscriptionType?> GetDefaultSubscriptionTypeAsync();
        Task<SubscriptionType?> GetSubscriptionTypeByNameAsync(string name);
        Task<SubscriptionType> AddSubscriptionTypeAsync(SubscriptionType subscriptionType);
        Task<SubscriptionType> UpdateSubscriptionTypeAsync(SubscriptionType subscriptionType);
        Task DeleteSubscriptionTypeAsync(Guid id);

        // Payment methods
        Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(Guid userId);
        Task<IEnumerable<Payment>> GetPaymentsBySubscriptionIdAsync(Guid subscriptionId);
        Task<Payment?> GetPaymentByIdAsync(Guid id);
        Task<Payment?> GetPaymentByStripePaymentIntentIdAsync(string stripePaymentIntentId);
        Task<Payment> AddPaymentAsync(Payment payment);
        Task<Payment> UpdatePaymentAsync(Payment payment);
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
