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
        Task<Subscription?> GetByIdAsync(int id);
        Task<Subscription?> GetByUserIdAsync(int userId);
        Task<Subscription?> GetActiveByUserIdAsync(int userId);
        Task<IEnumerable<Subscription>> GetByUserIdAllAsync(int userId);
        Task<Subscription?> GetByStripeSubscriptionIdAsync(string stripeSubscriptionId);
        Task<Subscription> AddAsync(Subscription subscription);
        Task<Subscription> UpdateAsync(Subscription subscription);
        Task DeleteAsync(int id);
        Task<bool> HasActiveSubscriptionAsync(int userId);
        Task<IEnumerable<Subscription>> GetExpiringSubscriptionsAsync(DateTime date);

        // SubscriptionType methods
        Task<IEnumerable<SubscriptionType>> GetAllSubscriptionTypesAsync();
        Task<SubscriptionType?> GetSubscriptionTypeByIdAsync(int id);
        Task<SubscriptionType?> GetDefaultSubscriptionTypeAsync();
        Task<SubscriptionType?> GetSubscriptionTypeByNameAsync(string name);
        Task<SubscriptionType> AddSubscriptionTypeAsync(SubscriptionType subscriptionType);
        Task<SubscriptionType> UpdateSubscriptionTypeAsync(SubscriptionType subscriptionType);
        Task DeleteSubscriptionTypeAsync(int id);

        // Payment methods
        Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(int userId);
        Task<IEnumerable<Payment>> GetPaymentsBySubscriptionIdAsync(int subscriptionId);
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<Payment?> GetPaymentByStripePaymentIntentIdAsync(string stripePaymentIntentId);
        Task<Payment> AddPaymentAsync(Payment payment);
        Task<Payment> UpdatePaymentAsync(Payment payment);
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
