using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Subscription;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    /// <summary>
    /// ES: Servicio para manejo de suscripciones y pagos
    /// EN: Service for subscription and payment management
    /// </summary>
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IStripePaymentService _stripePaymentService;
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;

        public SubscriptionService(
            ISubscriptionRepository subscriptionRepository,
            IStripePaymentService stripePaymentService,
            IUserRepository userRepository,
            ITeamRepository teamRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _stripePaymentService = stripePaymentService;
            _userRepository = userRepository;
            _teamRepository = teamRepository;
        }

        #region Subscription Type Management

        public async Task<ResponseContract<IEnumerable<SubscriptionTypeDto>>> GetAllSubscriptionTypesAsync()
        {
            try
            {
                var subscriptionTypes = await _subscriptionRepository.GetAllSubscriptionTypesAsync();

                var result = subscriptionTypes.Select(st => new SubscriptionTypeDto
                {
                    SubscriptionTypeId = st.SubscriptionTypeId,
                    Name = st.Name,
                    Description = st.Description,
                    MonthlyPrice = st.PriceInCents / 100m,
                    AnnualPrice = st.AnnualPriceInCents.HasValue ? st.AnnualPriceInCents.Value / 100m : null,
                    Features = st.Features,
                    TeamLimit = st.TeamLimit,
                    AthleteLimit = st.AthleteLimit,
                    MonthlyEvaluationLimit = st.MonthlyEvaluationLimit,
                    HasAdvancedStatistics = st.HasAdvancedStatistics,
                    HasPremiumChat = st.HasPremiumChat,
                    IsDefault = st.IsDefault
                }).ToList();

                return ResponseContract<IEnumerable<SubscriptionTypeDto>>.Ok(result, "Subscription types retrieved successfully");
            }
            catch (Exception ex)
            {
                return ResponseContract<IEnumerable<SubscriptionTypeDto>>.Fail($"Error retrieving subscription types: {ex.Message}");
            }
        }

        public async Task<ResponseContract<SubscriptionTypeDto>> GetSubscriptionTypeByIdAsync(int id)
        {
            try
            {
                var subscriptionType = await _subscriptionRepository.GetSubscriptionTypeByIdAsync(id);

                if (subscriptionType == null)
                {
                    return ResponseContract<SubscriptionTypeDto>.Fail("Subscription type not found");
                }

                var result = new SubscriptionTypeDto
                {
                    SubscriptionTypeId = subscriptionType.SubscriptionTypeId,
                    Name = subscriptionType.Name,
                    Description = subscriptionType.Description,
                    MonthlyPrice = subscriptionType.PriceInCents / 100m,
                    AnnualPrice = subscriptionType.AnnualPriceInCents.HasValue ? subscriptionType.AnnualPriceInCents.Value / 100m : null,
                    Features = subscriptionType.Features,
                    TeamLimit = subscriptionType.TeamLimit,
                    AthleteLimit = subscriptionType.AthleteLimit,
                    MonthlyEvaluationLimit = subscriptionType.MonthlyEvaluationLimit,
                    HasAdvancedStatistics = subscriptionType.HasAdvancedStatistics,
                    HasPremiumChat = subscriptionType.HasPremiumChat,
                    IsDefault = subscriptionType.IsDefault
                };

                return ResponseContract<SubscriptionTypeDto>.Ok(result, "Subscription type retrieved successfully");
            }
            catch (Exception ex)
            {
                return ResponseContract<SubscriptionTypeDto>.Fail($"Error retrieving subscription type: {ex.Message}");
            }
        }

        public async Task<ResponseContract<SubscriptionTypeDto>> CreateSubscriptionTypeAsync(CreateSubscriptionTypeDto createDto)
        {
            try
            {
                var subscriptionType = new SubscriptionType
                {
                    Name = createDto.Name,
                    Description = createDto.Description,
                    PriceInCents = (int)(createDto.MonthlyPrice * 100),
                    AnnualPriceInCents = createDto.AnnualPrice.HasValue ? (int)(createDto.AnnualPrice.Value * 100) : null,
                    Features = createDto.Features,
                    TeamLimit = createDto.TeamLimit,
                    AthleteLimit = createDto.AthleteLimit,
                    MonthlyEvaluationLimit = createDto.MonthlyEvaluationLimit,
                    HasAdvancedStatistics = createDto.HasAdvancedStatistics,
                    HasPremiumChat = createDto.HasPremiumChat,
                    IsActive = true,
                    IsDefault = false,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _subscriptionRepository.AddSubscriptionTypeAsync(subscriptionType);

                return ResponseContract<SubscriptionTypeDto>.Ok(
                    new SubscriptionTypeDto
                    {
                        SubscriptionTypeId = result.SubscriptionTypeId,
                        Name = result.Name,
                        Description = result.Description,
                        MonthlyPrice = result.PriceInCents / 100m,
                        AnnualPrice = result.AnnualPriceInCents.HasValue ? result.AnnualPriceInCents.Value / 100m : null,
                        Features = result.Features,
                        TeamLimit = result.TeamLimit,
                        AthleteLimit = result.AthleteLimit,
                        MonthlyEvaluationLimit = result.MonthlyEvaluationLimit,
                        HasAdvancedStatistics = result.HasAdvancedStatistics,
                        HasPremiumChat = result.HasPremiumChat,
                        IsDefault = result.IsDefault
                    },
                    "Subscription type created successfully"
                );
            }
            catch (Exception ex)
            {
                return ResponseContract<SubscriptionTypeDto>.Fail($"Error creating subscription type: {ex.Message}");
            }
        }

        public async Task<ResponseContract<SubscriptionTypeDto>> UpdateSubscriptionTypeAsync(int id, CreateSubscriptionTypeDto updateDto)
        {
            try
            {
                var subscriptionType = await _subscriptionRepository.GetSubscriptionTypeByIdAsync(id);
                if (subscriptionType == null)
                {
                    return ResponseContract<SubscriptionTypeDto>.Fail("Subscription type not found");
                }

                // Update properties from CreateSubscriptionTypeDto
                subscriptionType.Name = updateDto.Name;
                subscriptionType.Description = updateDto.Description;
                subscriptionType.PriceInCents = (int)(updateDto.MonthlyPrice * 100);
                subscriptionType.AnnualPriceInCents = updateDto.AnnualPrice.HasValue ? (int)(updateDto.AnnualPrice.Value * 100) : null;
                subscriptionType.Features = updateDto.Features;
                subscriptionType.TeamLimit = updateDto.TeamLimit;
                subscriptionType.AthleteLimit = updateDto.AthleteLimit;
                subscriptionType.MonthlyEvaluationLimit = updateDto.MonthlyEvaluationLimit;
                subscriptionType.HasAdvancedStatistics = updateDto.HasAdvancedStatistics;
                subscriptionType.HasPremiumChat = updateDto.HasPremiumChat;
                subscriptionType.UpdatedAt = DateTime.UtcNow;

                var result = await _subscriptionRepository.UpdateSubscriptionTypeAsync(subscriptionType);

                return ResponseContract<SubscriptionTypeDto>.Ok(
                    new SubscriptionTypeDto
                    {
                        SubscriptionTypeId = result.SubscriptionTypeId,
                        Name = result.Name,
                        Description = result.Description,
                        MonthlyPrice = result.PriceInCents / 100m,
                        AnnualPrice = result.AnnualPriceInCents.HasValue ? result.AnnualPriceInCents.Value / 100m : null,
                        Features = result.Features,
                        TeamLimit = result.TeamLimit,
                        AthleteLimit = result.AthleteLimit,
                        MonthlyEvaluationLimit = result.MonthlyEvaluationLimit,
                        HasAdvancedStatistics = result.HasAdvancedStatistics,
                        HasPremiumChat = result.HasPremiumChat,
                        IsDefault = result.IsDefault
                    },
                    "Subscription type updated successfully"
                );
            }
            catch (Exception ex)
            {
                return ResponseContract<SubscriptionTypeDto>.Fail($"Error updating subscription type: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> DeleteSubscriptionTypeAsync(int id)
        {
            try
            {
                await _subscriptionRepository.DeleteSubscriptionTypeAsync(id);
                return ResponseContract<bool>.Ok(true, "Subscription type deleted successfully");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error deleting subscription type: {ex.Message}");
            }
        }

        #endregion

        #region User Subscription Management

        public async Task<ResponseContract<UserSubscriptionDto>> GetUserSubscriptionAsync(int userId)
        {
            try
            {
                var subscription = await _subscriptionRepository.GetActiveByUserIdAsync(userId);

                if (subscription == null)
                {
                    // Get default free subscription
                    var defaultType = await _subscriptionRepository.GetDefaultSubscriptionTypeAsync();
                    if (defaultType != null)
                    {
                        return ResponseContract<UserSubscriptionDto>.Ok(
                            new UserSubscriptionDto
                            {
                                SubscriptionId = 0,
                                UserId = userId,
                                SubscriptionType = new SubscriptionTypeDto
                                {
                                    SubscriptionTypeId = defaultType.SubscriptionTypeId,
                                    Name = defaultType.Name,
                                    Description = defaultType.Description,
                                    MonthlyPrice = defaultType.PriceInCents / 100m,
                                    IsDefault = true,
                                    TeamLimit = defaultType.TeamLimit,
                                    AthleteLimit = defaultType.AthleteLimit,
                                    MonthlyEvaluationLimit = defaultType.MonthlyEvaluationLimit,
                                    HasAdvancedStatistics = defaultType.HasAdvancedStatistics,
                                    HasPremiumChat = defaultType.HasPremiumChat
                                },
                                Status = "free",
                                StartDate = DateTime.UtcNow,
                                Currency = "USD"
                            },
                            "Using default free subscription"
                        );
                    }
                }

                var user = await _userRepository.GetUserEntityByIdAsync(userId);

                var result = new UserSubscriptionDto
                {
                    SubscriptionId = subscription.SubscriptionId,
                    UserId = subscription.UserId,
                    SubscriptionType = new SubscriptionTypeDto
                    {
                        SubscriptionTypeId = subscription.SubscriptionType.SubscriptionTypeId,
                        Name = subscription.SubscriptionType.Name,
                        Description = subscription.SubscriptionType.Description,
                        MonthlyPrice = subscription.SubscriptionType.PriceInCents / 100m,
                        AnnualPrice = subscription.SubscriptionType.AnnualPriceInCents.HasValue ? subscription.SubscriptionType.AnnualPriceInCents.Value / 100m : null,
                        Features = subscription.SubscriptionType.Features,
                        TeamLimit = subscription.SubscriptionType.TeamLimit,
                        AthleteLimit = subscription.SubscriptionType.AthleteLimit,
                        MonthlyEvaluationLimit = subscription.SubscriptionType.MonthlyEvaluationLimit,
                        HasAdvancedStatistics = subscription.SubscriptionType.HasAdvancedStatistics,
                        HasPremiumChat = subscription.SubscriptionType.HasPremiumChat,
                        IsDefault = subscription.SubscriptionType.IsDefault
                    },
                    Status = subscription.Status,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    NextRenewalDate = subscription.NextRenewalDate,
                    IsTrial = subscription.IsTrial,
                    TrialEndDate = subscription.TrialEndDate,
                    IsAnnual = subscription.IsAnnual,
                    Currency = subscription.Currency
                };

                return ResponseContract<UserSubscriptionDto>.Ok(result, "User subscription retrieved successfully");
            }
            catch (Exception ex)
            {
                return ResponseContract<UserSubscriptionDto>.Fail($"Error retrieving user subscription: {ex.Message}");
            }
        }

        public async Task<ResponseContract<IEnumerable<UserSubscriptionDto>>> GetUserSubscriptionHistoryAsync(int userId)
        {
            try
            {
                var subscriptions = await _subscriptionRepository.GetByUserIdAllAsync(userId);
                var user = await _userRepository.GetUserEntityByIdAsync(userId);

                var result = subscriptions.Select(s => new UserSubscriptionDto
                {
                    SubscriptionId = s.SubscriptionId,
                    UserId = s.UserId,
                    SubscriptionType = new SubscriptionTypeDto
                    {
                        SubscriptionTypeId = s.SubscriptionType.SubscriptionTypeId,
                        Name = s.SubscriptionType.Name,
                        Description = s.SubscriptionType.Description,
                        MonthlyPrice = s.SubscriptionType.PriceInCents / 100m,
                        AnnualPrice = s.SubscriptionType.AnnualPriceInCents.HasValue ? s.SubscriptionType.AnnualPriceInCents.Value / 100m : null,
                        Features = s.SubscriptionType.Features,
                        TeamLimit = s.SubscriptionType.TeamLimit,
                        AthleteLimit = s.SubscriptionType.AthleteLimit,
                        MonthlyEvaluationLimit = s.SubscriptionType.MonthlyEvaluationLimit,
                        HasAdvancedStatistics = s.SubscriptionType.HasAdvancedStatistics,
                        HasPremiumChat = s.SubscriptionType.HasPremiumChat,
                        IsDefault = s.SubscriptionType.IsDefault
                    },
                    Status = s.Status,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    NextRenewalDate = s.NextRenewalDate,
                    IsTrial = s.IsTrial,
                    TrialEndDate = s.TrialEndDate,
                    IsAnnual = s.IsAnnual,
                    Currency = s.Currency
                }).ToList();

                return ResponseContract<IEnumerable<UserSubscriptionDto>>.Ok(result, "User subscription history retrieved successfully");
            }
            catch (Exception ex)
            {
                return ResponseContract<IEnumerable<UserSubscriptionDto>>.Fail($"Error retrieving user subscription history: {ex.Message}");
            }
        }

        public async Task<ResponseContract<SubscriptionCreateResponseDto>> CreateSubscriptionAsync(CreateSubscriptionDto createDto)
        {
            try
            {
                var user = await _userRepository.GetUserEntityByIdAsync(createDto.UserId);
                if (user == null)
                {
                    return ResponseContract<SubscriptionCreateResponseDto>.Fail("User not found");
                }

                var subscriptionType = await _subscriptionRepository.GetSubscriptionTypeByIdAsync(createDto.SubscriptionTypeId);
                if (subscriptionType == null)
                {
                    return ResponseContract<SubscriptionCreateResponseDto>.Fail("Subscription type not found");
                }

                // Check if user already has an active subscription
                var existingSubscription = await _subscriptionRepository.GetActiveByUserIdAsync(createDto.UserId);
                if (existingSubscription != null)
                {
                    return ResponseContract<SubscriptionCreateResponseDto>.Fail("User already has an active subscription");
                }

                var subscription = new Subscription
                {
                    UserId = createDto.UserId,
                    SubscriptionTypeId = createDto.SubscriptionTypeId,
                    Status = "pending",
                    StartDate = DateTime.UtcNow,
                    IsTrial = createDto.IsTrial,
                    IsAnnual = createDto.IsAnnual,
                    PricePaidInCents = createDto.IsAnnual ? subscriptionType.AnnualPriceInCents : subscriptionType.PriceInCents,
                    Currency = "USD", // Default currency since it's not in DTO
                    CreatedAt = DateTime.UtcNow
                };

                // Set trial end date if it's a trial
                if (createDto.IsTrial)
                {
                    subscription.TrialEndDate = DateTime.UtcNow.AddDays(createDto.TrialDays ?? 14);
                }

                // Set next renewal date
                if (createDto.IsAnnual)
                {
                    subscription.NextRenewalDate = DateTime.UtcNow.AddYears(1);
                }
                else
                {
                    subscription.NextRenewalDate = DateTime.UtcNow.AddMonths(1);
                }

                // If it's a free subscription, just create it
                if (subscriptionType.PriceInCents == 0)
                {
                    subscription.Status = "active";
                    var freeSubscription = await _subscriptionRepository.AddAsync(subscription);

                    return ResponseContract<SubscriptionCreateResponseDto>.Ok(
                        new SubscriptionCreateResponseDto
                        {
                            Success = true,
                            SubscriptionId = freeSubscription.SubscriptionId.ToString(),
                            Message = "Free subscription created successfully"
                        },
                        "Free subscription created successfully"
                    );
                }

                // For paid subscriptions, handle Stripe integration
                var customerResponse = await _stripePaymentService.GetOrCreateCustomerAsync(
                    createDto.UserId, 
                    user.Email!, 
                    $"{user.FirstName} {user.LastName}"
                );

                if (!customerResponse.Success)
                {
                    return ResponseContract<SubscriptionCreateResponseDto>.Fail("Failed to create Stripe customer");
                }

                subscription.StripeCustomerId = customerResponse.Data;

                // Get the appropriate price ID
                var priceId = createDto.IsAnnual ? subscriptionType.StripeAnnualPriceId : subscriptionType.StripeMonthlyPriceId;
                if (string.IsNullOrEmpty(priceId))
                {
                    return ResponseContract<SubscriptionCreateResponseDto>.Fail("Price not configured for this subscription type");
                }

                // Create Stripe subscription
                var stripeSubscriptionResponse = await _stripePaymentService.CreateStripeSubscriptionAsync(
                    customerResponse.Data, 
                    priceId, 
                    createDto.PaymentMethodId
                );

                if (!stripeSubscriptionResponse.Success)
                {
                    return ResponseContract<SubscriptionCreateResponseDto>.Fail("Failed to create Stripe subscription");
                }

                subscription.StripeSubscriptionId = stripeSubscriptionResponse.Data;
                subscription.Status = "active";

                var result = await _subscriptionRepository.AddAsync(subscription);

                return ResponseContract<SubscriptionCreateResponseDto>.Ok(
                    new SubscriptionCreateResponseDto
                    {
                        Success = true,
                        SubscriptionId = result.SubscriptionId.ToString(),
                        Message = "Subscription created successfully"
                    },
                    "Subscription created successfully"
                );
            }
            catch (Exception ex)
            {
                return ResponseContract<SubscriptionCreateResponseDto>.Fail($"Error creating subscription: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> CancelSubscriptionAsync(CancelSubscriptionDto cancelDto)
        {
            try
            {
                var subscription = await _subscriptionRepository.GetByIdAsync(cancelDto.SubscriptionId);
                if (subscription == null)
                {
                    return ResponseContract<bool>.Fail("Subscription not found");
                }

                // Cancel in Stripe if it's a paid subscription
                if (!string.IsNullOrEmpty(subscription.StripeSubscriptionId))
                {
                    var stripeResponse = await _stripePaymentService.CancelStripeSubscriptionAsync(
                        subscription.StripeSubscriptionId, 
                        cancelDto.CancelAtPeriodEnd
                    );

                    if (!stripeResponse.Success)
                    {
                        return ResponseContract<bool>.Fail("Failed to cancel Stripe subscription");
                    }
                }

                // Update local subscription
                subscription.Status = cancelDto.CancelAtPeriodEnd ? "cancel_at_period_end" : "canceled";
                subscription.CanceledAt = DateTime.UtcNow;

                if (!cancelDto.CancelAtPeriodEnd)
                {
                    subscription.EndDate = DateTime.UtcNow;
                }

                if (!string.IsNullOrEmpty(cancelDto.CancellationReason))
                {
                    subscription.Notes = cancelDto.CancellationReason;
                }

                await _subscriptionRepository.UpdateAsync(subscription);

                return ResponseContract<bool>.Ok(true, "Subscription canceled successfully");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error canceling subscription: {ex.Message}");
            }
        }

        public async Task<ResponseContract<UserSubscriptionDto>> UpdateSubscriptionAsync(UpdateSubscriptionDto updateDto)
        {
            try
            {
                var subscription = await _subscriptionRepository.GetByIdAsync(updateDto.SubscriptionId);
                if (subscription == null)
                {
                    return ResponseContract<UserSubscriptionDto>.Fail("Subscription not found");
                }

                var newSubscriptionType = await _subscriptionRepository.GetSubscriptionTypeByIdAsync(updateDto.NewSubscriptionTypeId);
                if (newSubscriptionType == null)
                {
                    return ResponseContract<UserSubscriptionDto>.Fail("New subscription type not found");
                }

                // Update Stripe subscription if necessary
                if (!string.IsNullOrEmpty(subscription.StripeSubscriptionId))
                {
                    var priceId = updateDto.IsAnnual ? newSubscriptionType.StripeAnnualPriceId : newSubscriptionType.StripeMonthlyPriceId;
                    if (!string.IsNullOrEmpty(priceId))
                    {
                        var stripeResponse = await _stripePaymentService.UpdateStripeSubscriptionAsync(
                            subscription.StripeSubscriptionId, 
                            priceId
                        );

                        if (!stripeResponse.Success)
                        {
                            return ResponseContract<UserSubscriptionDto>.Fail("Failed to update Stripe subscription");
                        }
                    }
                }

                // Update local subscription
                subscription.SubscriptionTypeId = updateDto.NewSubscriptionTypeId;
                subscription.IsAnnual = updateDto.IsAnnual;
                subscription.PricePaidInCents = updateDto.IsAnnual ? newSubscriptionType.AnnualPriceInCents : newSubscriptionType.PriceInCents;

                await _subscriptionRepository.UpdateAsync(subscription);

                // Return updated subscription
                return await GetUserSubscriptionAsync(subscription.UserId);
            }
            catch (Exception ex)
            {
                return ResponseContract<UserSubscriptionDto>.Fail($"Error updating subscription: {ex.Message}");
            }
        }

        public async Task<ResponseContract<UserSubscriptionDto>> ReactivateSubscriptionAsync(int subscriptionId)
        {
            try
            {
                var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
                if (subscription == null)
                {
                    return ResponseContract<UserSubscriptionDto>.Fail("Subscription not found");
                }

                subscription.Status = "active";
                subscription.CanceledAt = null;
                subscription.EndDate = null;

                // Set new renewal date
                if (subscription.IsAnnual)
                {
                    subscription.NextRenewalDate = DateTime.UtcNow.AddYears(1);
                }
                else
                {
                    subscription.NextRenewalDate = DateTime.UtcNow.AddMonths(1);
                }

                await _subscriptionRepository.UpdateAsync(subscription);

                return await GetUserSubscriptionAsync(subscription.UserId);
            }
            catch (Exception ex)
            {
                return ResponseContract<UserSubscriptionDto>.Fail($"Error reactivating subscription: {ex.Message}");
            }
        }

        public async Task<ResponseContract<UserSubscriptionDto>> StartTrialAsync(int userId, int subscriptionTypeId, int trialDays = 7)
        {
            try
            {
                var createDto = new CreateSubscriptionDto
                {
                    UserId = userId,
                    SubscriptionTypeId = subscriptionTypeId,
                    IsTrial = true,
                    TrialDays = trialDays,
                    IsAnnual = false
                };

                var createResponse = await CreateSubscriptionAsync(createDto);

                if (!createResponse.Success)
                {
                    return ResponseContract<UserSubscriptionDto>.Fail(createResponse.Message);
                }

                return await GetUserSubscriptionAsync(userId);
            }
            catch (Exception ex)
            {
                return ResponseContract<UserSubscriptionDto>.Fail($"Error starting trial: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> IsTrialAvailableAsync(int userId, int subscriptionTypeId)
        {
            try
            {
                var subscriptions = await _subscriptionRepository.GetByUserIdAllAsync(userId);
                var hasHadTrial = subscriptions.Any(s => s.SubscriptionTypeId == subscriptionTypeId && s.IsTrial);

                return ResponseContract<bool>.Ok(!hasHadTrial, hasHadTrial ? "Trial already used for this subscription type" : "Trial available");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error checking trial availability: {ex.Message}");
            }
        }

        #endregion

        #region Validation Methods

        public async Task<ResponseContract<bool>> ValidateUserSubscriptionAsync(int userId, string feature)
        {
            try
            {
                var subscription = await _subscriptionRepository.GetActiveByUserIdAsync(userId);
                if (subscription == null)
                {
                    return ResponseContract<bool>.Ok(false, "No active subscription found");
                }

                // Check if subscription is expired
                if (subscription.EndDate.HasValue && subscription.EndDate.Value < DateTime.UtcNow)
                {
                    return ResponseContract<bool>.Ok(false, "Subscription has expired");
                }

                // Check trial expiration
                if (subscription.IsTrial && subscription.TrialEndDate.HasValue && subscription.TrialEndDate.Value < DateTime.UtcNow)
                {
                    return ResponseContract<bool>.Ok(false, "Trial period has expired");
                }

                // Check feature access if provided
                if (!string.IsNullOrEmpty(feature))
                {
                    var featureAccess = await CanAccessFeatureAsync(userId, feature);
                    if (!featureAccess.Data)
                    {
                        return ResponseContract<bool>.Ok(false, $"Feature '{feature}' not available in current plan");
                    }
                }

                return ResponseContract<bool>.Ok(true, "Subscription is valid");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error validating subscription: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> HasActiveSubscriptionAsync(int userId)
        {
            try
            {
                var hasActive = await _subscriptionRepository.HasActiveSubscriptionAsync(userId);

                return ResponseContract<bool>.Ok(hasActive, hasActive ? "User has active subscription" : "User has no active subscription");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error checking active subscription: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> CanAccessFeatureAsync(int userId, string featureName)
        {
            try
            {
                var userSubscription = await GetUserSubscriptionAsync(userId);
                if (!userSubscription.Success)
                {
                    return ResponseContract<bool>.Fail("Could not retrieve user subscription");
                }

                var subscription = userSubscription.Data;
                
                return featureName.ToLower() switch
                {
                    "advanced_statistics" => ResponseContract<bool>.Ok(
                        subscription.SubscriptionType.HasAdvancedStatistics,
                        subscription.SubscriptionType.HasAdvancedStatistics ? "Feature available" : "Feature not available in current plan"
                    ),
                    "premium_chat" => ResponseContract<bool>.Ok(
                        subscription.SubscriptionType.HasPremiumChat,
                        subscription.SubscriptionType.HasPremiumChat ? "Feature available" : "Feature not available in current plan"
                    ),
                    _ => ResponseContract<bool>.Fail("Unknown feature")
                };
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error checking feature access: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> CanCreateTeamAsync(int userId)
        {
            try
            {
                var userSubscription = await GetUserSubscriptionAsync(userId);
                if (!userSubscription.Success)
                {
                    return ResponseContract<bool>.Fail("Could not retrieve user subscription");
                }

                var subscription = userSubscription.Data;
                
                // If no team limit, user can create unlimited teams
                if (!subscription.SubscriptionType.TeamLimit.HasValue)
                {
                    return ResponseContract<bool>.Ok(true, "No team limit in current plan");
                }

                // Count current teams
                var userTeamsCount = await _teamRepository.CountTeamsByUserIdAsync(userId);
                var canCreate = userTeamsCount < subscription.SubscriptionType.TeamLimit.Value;

                return ResponseContract<bool>.Ok(
                    canCreate,
                    canCreate 
                        ? "Can create more teams" 
                        : $"Team limit reached ({subscription.SubscriptionType.TeamLimit.Value})"
                );
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error checking team creation limit: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> CanAddAthleteToTeamAsync(int userId, int teamId)
        {
            try
            {
                var userSubscription = await GetUserSubscriptionAsync(userId);
                if (!userSubscription.Success)
                {
                    return ResponseContract<bool>.Fail("Could not retrieve user subscription");
                }

                var subscription = userSubscription.Data;
                
                // If no athlete limit, user can add unlimited athletes
                if (!subscription.SubscriptionType.AthleteLimit.HasValue)
                {
                    return ResponseContract<bool>.Ok(true, "No athlete limit in current plan");
                }

                // Count current athletes in team
                var athletesCount = await _teamRepository.CountAthletesByTeamIdAsync(teamId);
                var canAdd = athletesCount < subscription.SubscriptionType.AthleteLimit.Value;

                return ResponseContract<bool>.Ok(
                    canAdd,
                    canAdd 
                        ? "Can add more athletes to team" 
                        : $"Athlete limit reached ({subscription.SubscriptionType.AthleteLimit.Value})"
                );
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error checking athlete limit: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> CanAddAthleteAsync(int userId, int teamId)
        {
            try
            {
                var userSubscription = await GetUserSubscriptionAsync(userId);
                if (!userSubscription.Success)
                {
                    return ResponseContract<bool>.Fail("Could not retrieve user subscription");
                }

                var subscription = userSubscription.Data;
                
                // If no athlete limit, user can add unlimited athletes
                if (!subscription.SubscriptionType.AthleteLimit.HasValue)
                {
                    return ResponseContract<bool>.Ok(true, "No athlete limit in current plan");
                }

                // Count current athletes in team
                var athletesCount = await _teamRepository.CountAthletesByTeamIdAsync(teamId);
                var canAdd = athletesCount < subscription.SubscriptionType.AthleteLimit.Value;

                return ResponseContract<bool>.Ok(
                    canAdd,
                    canAdd 
                        ? "Can add more athletes to team" 
                        : $"Athlete limit reached ({subscription.SubscriptionType.AthleteLimit.Value})"
                );
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error checking athlete limit: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> CanPerformEvaluationAsync(int userId)
        {
            try
            {
                var userSubscription = await GetUserSubscriptionAsync(userId);
                if (!userSubscription.Success)
                {
                    return ResponseContract<bool>.Fail("Could not retrieve user subscription");
                }

                var subscription = userSubscription.Data;
                
                // If no evaluation limit, user can perform unlimited evaluations
                if (!subscription.SubscriptionType.MonthlyEvaluationLimit.HasValue)
                {
                    return ResponseContract<bool>.Ok(true, "No evaluation limit in current plan");
                }

                // Count current month's evaluations
                var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                
                // This would need to be implemented in a repository that tracks evaluations
                // For now, returning true as placeholder
                var evaluationsThisMonth = 0; // await _evaluationRepository.CountByUserIdAndDateRangeAsync(userId, startOfMonth, endOfMonth);
                var canPerform = evaluationsThisMonth < subscription.SubscriptionType.MonthlyEvaluationLimit.Value;

                return ResponseContract<bool>.Ok(
                    canPerform,
                    canPerform 
                        ? "Can perform more evaluations this month"
                        : $"Monthly evaluation limit reached ({subscription.SubscriptionType.MonthlyEvaluationLimit.Value})"
                );
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error checking evaluation limit: {ex.Message}");
            }
        }

        public async Task<ResponseContract<int>> GetRemainingTeamsAsync(int userId)
        {
            try
            {
                var userSubscription = await GetUserSubscriptionAsync(userId);
                if (!userSubscription.Success)
                {
                    return ResponseContract<int>.Fail("Could not retrieve user subscription");
                }

                var subscription = userSubscription.Data;
                
                // If no team limit, return max value
                if (!subscription.SubscriptionType.TeamLimit.HasValue)
                {
                    return ResponseContract<int>.Ok(int.MaxValue, "Unlimited teams available");
                }

                // Count current teams
                var userTeamsCount = await _teamRepository.CountTeamsByUserIdAsync(userId);
                var remaining = Math.Max(0, subscription.SubscriptionType.TeamLimit.Value - userTeamsCount);

                return ResponseContract<int>.Ok(remaining, $"{remaining} teams remaining");
            }
            catch (Exception ex)
            {
                return ResponseContract<int>.Fail($"Error getting remaining teams: {ex.Message}");
            }
        }

        public async Task<ResponseContract<int>> GetRemainingAthletesAsync(int userId, int teamId)
        {
            try
            {
                var userSubscription = await GetUserSubscriptionAsync(userId);
                if (!userSubscription.Success)
                {
                    return ResponseContract<int>.Fail("Could not retrieve user subscription");
                }

                var subscription = userSubscription.Data;
                
                // If no athlete limit, return max value
                if (!subscription.SubscriptionType.AthleteLimit.HasValue)
                {
                    return ResponseContract<int>.Ok(int.MaxValue, "Unlimited athletes available");
                }

                // Count current athletes in team
                var athletesCount = await _teamRepository.CountAthletesByTeamIdAsync(teamId);
                var remaining = Math.Max(0, subscription.SubscriptionType.AthleteLimit.Value - athletesCount);

                return ResponseContract<int>.Ok(remaining, $"{remaining} athletes remaining");
            }
            catch (Exception ex)
            {
                return ResponseContract<int>.Fail($"Error getting remaining athletes: {ex.Message}");
            }
        }

        public async Task<ResponseContract<int>> GetRemainingEvaluationsAsync(int userId)
        {
            try
            {
                var userSubscription = await GetUserSubscriptionAsync(userId);
                if (!userSubscription.Success)
                {
                    return ResponseContract<int>.Fail("Could not retrieve user subscription");
                }

                var subscription = userSubscription.Data;
                
                // If no evaluation limit, return max value
                if (!subscription.SubscriptionType.MonthlyEvaluationLimit.HasValue)
                {
                    return ResponseContract<int>.Ok(int.MaxValue, "Unlimited evaluations available");
                }

                // This would need evaluation repository implementation
                // For now, returning the full limit
                var remaining = subscription.SubscriptionType.MonthlyEvaluationLimit.Value;

                return ResponseContract<int>.Ok(remaining, $"{remaining} evaluations remaining this month");
            }
            catch (Exception ex)
            {
                return ResponseContract<int>.Fail($"Error getting remaining evaluations: {ex.Message}");
            }
        }

        #endregion

        #region Webhook Handling

        public async Task<ResponseContract<bool>> HandleStripeWebhookAsync(string eventType, object eventData)
        {
            try
            {
                return eventType switch
                {
                    "customer.subscription.updated" => await ProcessSubscriptionUpdatedAsync(GetSubscriptionIdFromEvent(eventData)),
                    "invoice.payment_succeeded" => await ProcessPaymentSucceededAsync(GetPaymentIntentIdFromEvent(eventData)),
                    "invoice.payment_failed" => await ProcessPaymentFailedAsync(GetPaymentIntentIdFromEvent(eventData)),
                    _ => ResponseContract<bool>.Ok(true, $"Webhook event '{eventType}' not handled but acknowledged")
                };
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error handling webhook: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> ProcessSubscriptionUpdatedAsync(string stripeSubscriptionId)
        {
            try
            {
                if (string.IsNullOrEmpty(stripeSubscriptionId))
                {
                    return ResponseContract<bool>.Fail("Stripe subscription ID is required");
                }

                var subscription = await _subscriptionRepository.GetByStripeSubscriptionIdAsync(stripeSubscriptionId);
                if (subscription == null)
                {
                    return ResponseContract<bool>.Fail("Subscription not found");
                }

                // Get updated subscription from Stripe
                var stripeResponse = await _stripePaymentService.GetStripeSubscriptionAsync(stripeSubscriptionId);
                if (!stripeResponse.Success)
                {
                    return ResponseContract<bool>.Fail("Failed to get Stripe subscription");
                }

                // Update subscription status and dates based on Stripe data
                // This would need proper implementation based on Stripe subscription object
                subscription.Status = "active"; // This should come from Stripe
                await _subscriptionRepository.UpdateAsync(subscription);

                return ResponseContract<bool>.Ok(true, "Subscription updated successfully");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error processing subscription update: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> ProcessPaymentSucceededAsync(string stripePaymentIntentId)
        {
            try
            {
                if (string.IsNullOrEmpty(stripePaymentIntentId))
                {
                    return ResponseContract<bool>.Fail("Stripe payment intent ID is required");
                }

                var payment = await _subscriptionRepository.GetPaymentByStripePaymentIntentIdAsync(stripePaymentIntentId);
                if (payment != null)
                {
                    payment.Status = "succeeded";
                    payment.ProcessedAt = DateTime.UtcNow;
                    await _subscriptionRepository.UpdatePaymentAsync(payment);
                }

                return ResponseContract<bool>.Ok(true, "Payment success processed");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error processing payment success: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> ProcessPaymentFailedAsync(string stripePaymentIntentId)
        {
            try
            {
                if (string.IsNullOrEmpty(stripePaymentIntentId))
                {
                    return ResponseContract<bool>.Fail("Stripe payment intent ID is required");
                }

                var payment = await _subscriptionRepository.GetPaymentByStripePaymentIntentIdAsync(stripePaymentIntentId);
                if (payment != null)
                {
                    payment.Status = "failed";
                    await _subscriptionRepository.UpdatePaymentAsync(payment);
                }

                return ResponseContract<bool>.Ok(true, "Payment failure processed");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error processing payment failure: {ex.Message}");
            }
        }

        #endregion

        #region Helper Methods

        private string GetSubscriptionIdFromEvent(object eventData)
        {
            // TODO: Extract subscription ID from Stripe event data
            // Implementation depends on the actual Stripe event structure
            _ = eventData; // Suppress unused parameter warning
            return "";
        }

        private string GetPaymentIntentIdFromEvent(object eventData)
        {
            // TODO: Extract payment intent ID from Stripe event data  
            // Implementation depends on the actual Stripe event structure
            _ = eventData; // Suppress unused parameter warning
            return "";
        }

        #endregion
    }
}
