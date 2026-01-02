using BocciaCoaching.Data;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories
{
    /// <summary>
    /// ES: Repositorio para manejo de suscripciones
    /// EN: Repository for subscription management
    /// </summary>
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Subscription Methods

        public async Task<IEnumerable<Subscription>> GetAllAsync()
        {
            return await _context.Subscriptions
                .Include(s => s.User)
                .Include(s => s.SubscriptionType)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<Subscription?> GetByIdAsync(int id)
        {
            return await _context.Subscriptions
                .Include(s => s.User)
                .Include(s => s.SubscriptionType)
                .Include(s => s.Payments)
                .FirstOrDefaultAsync(s => s.SubscriptionId == id);
        }

        public async Task<Subscription?> GetByUserIdAsync(int userId)
        {
            return await _context.Subscriptions
                .Include(s => s.SubscriptionType)
                .Include(s => s.Payments)
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<Subscription?> GetActiveByUserIdAsync(int userId)
        {
            return await _context.Subscriptions
                .Include(s => s.SubscriptionType)
                .Include(s => s.Payments)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Status == "active");
        }

        public async Task<IEnumerable<Subscription>> GetByUserIdAllAsync(int userId)
        {
            return await _context.Subscriptions
                .Include(s => s.SubscriptionType)
                .Include(s => s.Payments)
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<Subscription?> GetByStripeSubscriptionIdAsync(string stripeSubscriptionId)
        {
            return await _context.Subscriptions
                .Include(s => s.User)
                .Include(s => s.SubscriptionType)
                .FirstOrDefaultAsync(s => s.StripeSubscriptionId == stripeSubscriptionId);
        }

        public async Task<Subscription> AddAsync(Subscription subscription)
        {
            subscription.CreatedAt = DateTime.UtcNow;
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }

        public async Task<Subscription> UpdateAsync(Subscription subscription)
        {
            subscription.UpdatedAt = DateTime.UtcNow;
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }

        public async Task DeleteAsync(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> HasActiveSubscriptionAsync(int userId)
        {
            return await _context.Subscriptions
                .AnyAsync(s => s.UserId == userId && s.Status == "active");
        }

        public async Task<IEnumerable<Subscription>> GetExpiringSubscriptionsAsync(DateTime date)
        {
            return await _context.Subscriptions
                .Include(s => s.User)
                .Include(s => s.SubscriptionType)
                .Where(s => s.Status == "active" && 
                           s.NextRenewalDate.HasValue && 
                           s.NextRenewalDate.Value.Date == date.Date)
                .ToListAsync();
        }

        #endregion

        #region SubscriptionType Methods

        public async Task<IEnumerable<SubscriptionType>> GetAllSubscriptionTypesAsync()
        {
            return await _context.SubscriptionTypes
                .Where(st => st.IsActive)
                .OrderBy(st => st.PriceInCents)
                .ToListAsync();
        }

        public async Task<SubscriptionType?> GetSubscriptionTypeByIdAsync(int id)
        {
            return await _context.SubscriptionTypes
                .FirstOrDefaultAsync(st => st.SubscriptionTypeId == id && st.IsActive);
        }

        public async Task<SubscriptionType?> GetDefaultSubscriptionTypeAsync()
        {
            return await _context.SubscriptionTypes
                .FirstOrDefaultAsync(st => st.IsDefault && st.IsActive);
        }

        public async Task<SubscriptionType?> GetSubscriptionTypeByNameAsync(string name)
        {
            return await _context.SubscriptionTypes
                .FirstOrDefaultAsync(st => st.Name.ToLower() == name.ToLower() && st.IsActive);
        }

        public async Task<SubscriptionType> AddSubscriptionTypeAsync(SubscriptionType subscriptionType)
        {
            subscriptionType.CreatedAt = DateTime.UtcNow;
            _context.SubscriptionTypes.Add(subscriptionType);
            await _context.SaveChangesAsync();
            return subscriptionType;
        }

        public async Task<SubscriptionType> UpdateSubscriptionTypeAsync(SubscriptionType subscriptionType)
        {
            subscriptionType.UpdatedAt = DateTime.UtcNow;
            _context.SubscriptionTypes.Update(subscriptionType);
            await _context.SaveChangesAsync();
            return subscriptionType;
        }

        public async Task DeleteSubscriptionTypeAsync(int id)
        {
            var subscriptionType = await _context.SubscriptionTypes.FindAsync(id);
            if (subscriptionType != null)
            {
                subscriptionType.IsActive = false;
                subscriptionType.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        #endregion

        #region Payment Methods

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(int userId)
        {
            return await _context.Payments
                .Include(p => p.Subscription)
                .ThenInclude(s => s!.SubscriptionType)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsBySubscriptionIdAsync(int subscriptionId)
        {
            return await _context.Payments
                .Where(p => p.SubscriptionId == subscriptionId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Subscription)
                .ThenInclude(s => s!.SubscriptionType)
                .FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        public async Task<Payment?> GetPaymentByStripePaymentIntentIdAsync(string stripePaymentIntentId)
        {
            return await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Subscription)
                .FirstOrDefaultAsync(p => p.StripePaymentIntentId == stripePaymentIntentId);
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            payment.CreatedAt = DateTime.UtcNow;
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> UpdatePaymentAsync(Payment payment)
        {
            payment.UpdatedAt = DateTime.UtcNow;
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Subscription)
                .ThenInclude(s => s!.SubscriptionType)
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        #endregion
    }
}
