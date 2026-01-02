using BocciaCoaching.Models.Configuration;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Payment;
using BocciaCoaching.Services.Interfaces;
using Microsoft.Extensions.Options;
using Stripe;

namespace BocciaCoaching.Services
{
    /// <summary>
    /// ES: Servicio para manejo de pagos con Stripe
    /// EN: Service for Stripe payment handling
    /// </summary>
    public class StripePaymentService : IStripePaymentService
    {
        private readonly StripeSettings _stripeSettings;
        private readonly PaymentIntentService _paymentIntentService;
        private readonly CustomerService _customerService;
        private readonly Stripe.SubscriptionService _subscriptionService; // Namespace completo
        private readonly ProductService _productService;
        private readonly PriceService _priceService;
        private readonly RefundService _refundService;
        private readonly InvoiceService _invoiceService;

        public StripePaymentService(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

            _paymentIntentService = new PaymentIntentService();
            _customerService = new CustomerService();
            _subscriptionService = new Stripe.SubscriptionService(); // Especificar namespace completo
            _productService = new ProductService();
            _priceService = new PriceService();
            _refundService = new RefundService();
            _invoiceService = new InvoiceService();
        }

        #region Payment Intent Methods

        public async Task<ResponseContract<PaymentIntentResponseDto>> CreatePaymentIntentAsync(CreatePaymentIntentDto createDto)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(createDto.IsAnnual ? GetAnnualPrice(createDto.SubscriptionTypeId) : GetMonthlyPrice(createDto.SubscriptionTypeId)) * 100,
                    Currency = createDto.Currency.ToLower(),
                    PaymentMethod = createDto.PaymentMethodId,
                    Metadata = new Dictionary<string, string>
                    {
                        {"user_id", createDto.UserId.ToString()},
                        {"subscription_type_id", createDto.SubscriptionTypeId.ToString()},
                        {"is_annual", createDto.IsAnnual.ToString()}
                    },
                    ConfirmationMethod = "manual",
                    Confirm = createDto.ConfirmPayment
                };

                var paymentIntent = await _paymentIntentService.CreateAsync(options);

                return ResponseContract<PaymentIntentResponseDto>.Ok(
                    new PaymentIntentResponseDto
                    {
                        Success = true,
                        ClientSecret = paymentIntent.ClientSecret,
                        PaymentIntentId = paymentIntent.Id,
                        Status = paymentIntent.Status,
                        Amount = paymentIntent.Amount / 100m,
                        Currency = paymentIntent.Currency,
                        RequiresAction = paymentIntent.Status == "requires_action"
                    },
                    "Payment intent created successfully"
                );
            }
            catch (StripeException ex)
            {
                return ResponseContract<PaymentIntentResponseDto>.Fail($"Stripe error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return ResponseContract<PaymentIntentResponseDto>.Fail($"Error creating payment intent: {ex.Message}");
            }
        }

        public async Task<ResponseContract<PaymentIntentResponseDto>> ConfirmPaymentIntentAsync(ConfirmPaymentDto confirmDto)
        {
            try
            {
                var options = new PaymentIntentConfirmOptions
                {
                    PaymentMethod = confirmDto.PaymentMethodId
                };

                var paymentIntent = await _paymentIntentService.ConfirmAsync(confirmDto.PaymentIntentId, options);

                return ResponseContract<PaymentIntentResponseDto>.Ok(
                    new PaymentIntentResponseDto
                    {
                        Success = paymentIntent.Status == "succeeded",
                        PaymentIntentId = paymentIntent.Id,
                        Status = paymentIntent.Status,
                        Amount = paymentIntent.Amount / 100m,
                        Currency = paymentIntent.Currency,
                        RequiresAction = paymentIntent.Status == "requires_action"
                    },
                    paymentIntent.Status == "succeeded" ? "Payment confirmed successfully" : $"Payment status: {paymentIntent.Status}"
                );
            }
            catch (StripeException ex)
            {
                return ResponseContract<PaymentIntentResponseDto>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<PaymentIntentResponseDto>> GetPaymentIntentAsync(string paymentIntentId)
        {
            try
            {
                var paymentIntent = await _paymentIntentService.GetAsync(paymentIntentId);

                return ResponseContract<PaymentIntentResponseDto>.Ok(
                    new PaymentIntentResponseDto
                    {
                        Success = true,
                        PaymentIntentId = paymentIntent.Id,
                        Status = paymentIntent.Status,
                        Amount = paymentIntent.Amount / 100m,
                        Currency = paymentIntent.Currency
                    },
                    "Payment intent retrieved successfully"
                );
            }
            catch (StripeException ex)
            {
                return ResponseContract<PaymentIntentResponseDto>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> CancelPaymentIntentAsync(string paymentIntentId)
        {
            try
            {
                await _paymentIntentService.CancelAsync(paymentIntentId);
                return ResponseContract<bool>.Ok(true, "Payment intent canceled successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<bool>.Fail($"Stripe error: {ex.Message}");
            }
        }

        #endregion

        #region Customer Methods

        public async Task<ResponseContract<string>> CreateCustomerAsync(int userId, string email, string? name = null)
        {
            try
            {
                var options = new CustomerCreateOptions
                {
                    Email = email,
                    Name = name,
                    Metadata = new Dictionary<string, string>
                    {
                        {"user_id", userId.ToString()}
                    }
                };

                var customer = await _customerService.CreateAsync(options);

                return ResponseContract<string>.Ok(customer.Id, "Customer created successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<string>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<string>> GetOrCreateCustomerAsync(int userId, string email, string? name = null)
        {
            try
            {
                // First, try to find existing customer by metadata
                var listOptions = new CustomerListOptions
                {
                    Email = email,
                    Limit = 1
                };

                var customers = await _customerService.ListAsync(listOptions);
                var existingCustomer = customers.FirstOrDefault(c => 
                    c.Metadata.ContainsKey("user_id") && 
                    c.Metadata["user_id"] == userId.ToString());

                if (existingCustomer != null)
                {
                    return ResponseContract<string>.Ok(existingCustomer.Id, "Existing customer found");
                }

                // Create new customer if not found
                return await CreateCustomerAsync(userId, email, name);
            }
            catch (StripeException ex)
            {
                return ResponseContract<string>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> UpdateCustomerAsync(string customerId, string? email = null, string? name = null)
        {
            try
            {
                var options = new CustomerUpdateOptions();
                
                if (!string.IsNullOrEmpty(email))
                    options.Email = email;
                    
                if (!string.IsNullOrEmpty(name))
                    options.Name = name;

                await _customerService.UpdateAsync(customerId, options);

                return ResponseContract<bool>.Ok(true, "Customer updated successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<bool>.Fail($"Stripe error: {ex.Message}");
            }
        }

        #endregion

        #region Subscription Methods

        public async Task<ResponseContract<string>> CreateStripeSubscriptionAsync(string customerId, string priceId, string? paymentMethodId = null)
        {
            try
            {
                var options = new SubscriptionCreateOptions
                {
                    Customer = customerId,
                    Items = new List<SubscriptionItemOptions>
                    {
                        new SubscriptionItemOptions
                        {
                            Price = priceId
                        }
                    },
                    PaymentBehavior = "default_incomplete",
                    PaymentSettings = new SubscriptionPaymentSettingsOptions
                    {
                        SaveDefaultPaymentMethod = "on_subscription"
                    },
                    Expand = new List<string> { "latest_invoice.payment_intent" }
                };

                if (!string.IsNullOrEmpty(paymentMethodId))
                {
                    options.DefaultPaymentMethod = paymentMethodId;
                }

                var subscription = await _subscriptionService.CreateAsync(options);

                return ResponseContract<string>.Ok(subscription.Id, "Stripe subscription created successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<string>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> CancelStripeSubscriptionAsync(string subscriptionId, bool atPeriodEnd = true)
        {
            try
            {
                if (atPeriodEnd)
                {
                    var updateOptions = new SubscriptionUpdateOptions
                    {
                        CancelAtPeriodEnd = true
                    };
                    await _subscriptionService.UpdateAsync(subscriptionId, updateOptions);
                }
                else
                {
                    await _subscriptionService.CancelAsync(subscriptionId);
                }

                return ResponseContract<bool>.Ok(true, "Subscription canceled successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<bool>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> UpdateStripeSubscriptionAsync(string subscriptionId, string newPriceId)
        {
            try
            {
                var subscription = await _subscriptionService.GetAsync(subscriptionId);
                var subscriptionItem = subscription.Items.Data.First();

                var options = new SubscriptionUpdateOptions
                {
                    Items = new List<SubscriptionItemOptions>
                    {
                        new SubscriptionItemOptions
                        {
                            Id = subscriptionItem.Id,
                            Price = newPriceId
                        }
                    }
                };

                await _subscriptionService.UpdateAsync(subscriptionId, options);

                return ResponseContract<bool>.Ok(true, "Subscription updated successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<bool>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<object>> GetStripeSubscriptionAsync(string subscriptionId)
        {
            try
            {
                var subscription = await _subscriptionService.GetAsync(subscriptionId);
                return ResponseContract<object>.Ok(subscription, "Subscription retrieved successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<object>.Fail($"Stripe error: {ex.Message}");
            }
        }

        #endregion

        #region Product and Price Methods

        public async Task<ResponseContract<string>> CreateProductAsync(string name, string description)
        {
            try
            {
                var options = new ProductCreateOptions
                {
                    Name = name,
                    Description = description
                };

                var product = await _productService.CreateAsync(options);

                return ResponseContract<string>.Ok(product.Id, "Product created successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<string>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<string>> CreatePriceAsync(string productId, long unitAmount, string currency = "USD", string interval = "month")
        {
            try
            {
                var options = new PriceCreateOptions
                {
                    Product = productId,
                    UnitAmount = unitAmount,
                    Currency = currency,
                    Recurring = new PriceRecurringOptions
                    {
                        Interval = interval
                    }
                };

                var price = await _priceService.CreateAsync(options);

                return ResponseContract<string>.Ok(price.Id, "Price created successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<string>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> UpdateProductAsync(string productId, string? name = null, string? description = null)
        {
            try
            {
                var options = new ProductUpdateOptions();
                
                if (!string.IsNullOrEmpty(name))
                    options.Name = name;
                    
                if (!string.IsNullOrEmpty(description))
                    options.Description = description;

                await _productService.UpdateAsync(productId, options);

                return ResponseContract<bool>.Ok(true, "Product updated successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<bool>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> DeactivatePriceAsync(string priceId)
        {
            try
            {
                var options = new PriceUpdateOptions
                {
                    Active = false
                };

                await _priceService.UpdateAsync(priceId, options);

                return ResponseContract<bool>.Ok(true, "Price deactivated successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<bool>.Fail($"Stripe error: {ex.Message}");
            }
        }

        #endregion

        #region Webhook Methods

        public async Task<ResponseContract<object>> ConstructWebhookEventAsync(string json, string signature)
        {
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json, 
                    signature, 
                    _stripeSettings.WebhookSecret
                );

                await Task.CompletedTask;
                return ResponseContract<object>.Ok(stripeEvent, "Webhook event constructed successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<object>.Fail($"Stripe webhook error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> ProcessWebhookEventAsync(string eventType, object eventData)
        {
            // This will be handled by the subscription service
            await Task.CompletedTask;
            
            return ResponseContract<bool>.Ok(true, "Webhook event processed");
        }

        #endregion

        #region Refund Methods

        public async Task<ResponseContract<string>> CreateRefundAsync(string paymentIntentId, long? amount = null)
        {
            try
            {
                var options = new RefundCreateOptions
                {
                    PaymentIntent = paymentIntentId
                };

                if (amount.HasValue)
                    options.Amount = amount;

                var refund = await _refundService.CreateAsync(options);

                return ResponseContract<string>.Ok(refund.Id, "Refund created successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<string>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<object>> GetRefundAsync(string refundId)
        {
            try
            {
                var refund = await _refundService.GetAsync(refundId);
                return ResponseContract<object>.Ok(refund, "Refund retrieved successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<object>.Fail($"Stripe error: {ex.Message}");
            }
        }

        #endregion

        #region Invoice Methods

        public async Task<ResponseContract<string>> CreateInvoiceAsync(string customerId)
        {
            try
            {
                var options = new InvoiceCreateOptions
                {
                    Customer = customerId
                };

                var invoice = await _invoiceService.CreateAsync(options);

                return ResponseContract<string>.Ok(invoice.Id, "Invoice created successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<string>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> FinalizeInvoiceAsync(string invoiceId)
        {
            try
            {
                await _invoiceService.FinalizeInvoiceAsync(invoiceId);
                return ResponseContract<bool>.Ok(true, "Invoice finalized successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<bool>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<object>> GetInvoiceAsync(string invoiceId)
        {
            try
            {
                var invoice = await _invoiceService.GetAsync(invoiceId);
                return ResponseContract<object>.Ok(invoice, "Invoice retrieved successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<object>.Fail($"Stripe error: {ex.Message}");
            }
        }

        public async Task<ResponseContract<object>> GetUpcomingInvoiceAsync(string customerId)
        {
            try
            {
                var options = new UpcomingInvoiceOptions
                {
                    Customer = customerId
                };

                var invoice = await _invoiceService.UpcomingAsync(options);
                return ResponseContract<object>.Ok(invoice, "Upcoming invoice retrieved successfully");
            }
            catch (StripeException ex)
            {
                return ResponseContract<object>.Fail($"Stripe error: {ex.Message}");
            }
        }

        #endregion

        #region Usage Methods

        public async Task<ResponseContract<bool>> RecordUsageAsync(string subscriptionItemId, int quantity)
        {
            try
            {
                // TODO: Implementar cuando se necesite facturación por uso
                await Task.CompletedTask;

                return ResponseContract<bool>.Ok(true, "Usage recording not implemented yet");
            }
            catch (StripeException ex)
            {
                return ResponseContract<bool>.Fail($"Stripe error: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

        private decimal GetMonthlyPrice(int subscriptionTypeId)
        {
            // TODO: Obtener desde base de datos
            // var subscriptionType = await _subscriptionRepository.GetSubscriptionTypeByIdAsync(subscriptionTypeId);
            // return subscriptionType?.PriceInCents / 100m ?? 0;
            
            // Valores por defecto hasta que se implemente la integración con base de datos
            return subscriptionTypeId switch
            {
                1 => 0m,      // Free
                2 => 9.99m,   // Premium
                3 => 19.99m,  // Pro
                _ => 9.99m
            };
        }

        private decimal GetAnnualPrice(int subscriptionTypeId)
        {
            // TODO: Obtener desde base de datos
            // var subscriptionType = await _subscriptionRepository.GetSubscriptionTypeByIdAsync(subscriptionTypeId);
            // return subscriptionType?.AnnualPriceInCents / 100m ?? (GetMonthlyPrice(subscriptionTypeId) * 10);
            
            // Valores por defecto hasta que se implemente la integración con base de datos
            return subscriptionTypeId switch
            {
                1 => 0m,       // Free
                2 => 99.90m,   // Premium (2 meses gratis)
                3 => 199.90m,  // Pro (2 meses gratis)
                _ => GetMonthlyPrice(subscriptionTypeId) * 10
            };
        }

        #endregion
    }
}
