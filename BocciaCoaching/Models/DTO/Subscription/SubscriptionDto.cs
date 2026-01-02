namespace BocciaCoaching.Models.DTO.Subscription
{
    /// <summary>
    /// ES: DTO para mostrar información básica de tipo de suscripción
    /// EN: DTO for basic subscription type information
    /// </summary>
    public class SubscriptionTypeDto
    {
        public int SubscriptionTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal MonthlyPrice { get; set; }
        public decimal? AnnualPrice { get; set; }
        public string? Features { get; set; }
        public int? TeamLimit { get; set; }
        public int? AthleteLimit { get; set; }
        public int? MonthlyEvaluationLimit { get; set; }
        public bool HasAdvancedStatistics { get; set; }
        public bool HasPremiumChat { get; set; }
        public bool IsDefault { get; set; }
    }

    /// <summary>
    /// ES: DTO para crear un nuevo tipo de suscripción
    /// EN: DTO for creating a new subscription type
    /// </summary>
    public class CreateSubscriptionTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal MonthlyPrice { get; set; }
        public decimal? AnnualPrice { get; set; }
        public string? Features { get; set; }
        public int? TeamLimit { get; set; }
        public int? AthleteLimit { get; set; }
        public int? MonthlyEvaluationLimit { get; set; }
        public bool HasAdvancedStatistics { get; set; }
        public bool HasPremiumChat { get; set; }
    }

    /// <summary>
    /// ES: DTO para información de suscripción del usuario
    /// EN: DTO for user subscription information
    /// </summary>
    public class UserSubscriptionDto
    {
        public int SubscriptionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public SubscriptionTypeDto SubscriptionType { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? NextRenewalDate { get; set; }
        public bool IsTrial { get; set; }
        public DateTime? TrialEndDate { get; set; }
        public bool IsAnnual { get; set; }
        public decimal? PricePaid { get; set; }
        public string Currency { get; set; } = string.Empty;
    }

    /// <summary>
    /// ES: DTO para crear una nueva suscripción
    /// EN: DTO for creating a new subscription
    /// </summary>
    public class CreateSubscriptionDto
    {
        public int UserId { get; set; }
        public int SubscriptionTypeId { get; set; }
        public bool IsAnnual { get; set; } = false;
        public bool IsTrial { get; set; } = false;
        public int? TrialDays { get; set; }
        public string? PaymentMethodId { get; set; }
    }

    /// <summary>
    /// ES: DTO para respuesta de creación de suscripción
    /// EN: DTO for subscription creation response
    /// </summary>
    public class SubscriptionCreateResponseDto
    {
        public bool Success { get; set; }
        public string? ClientSecret { get; set; }
        public string? SubscriptionId { get; set; }
        public string? Message { get; set; }
        public UserSubscriptionDto? Subscription { get; set; }
    }

    /// <summary>
    /// ES: DTO para cancelar suscripción
    /// EN: DTO for canceling subscription
    /// </summary>
    public class CancelSubscriptionDto
    {
        public int SubscriptionId { get; set; }
        public bool CancelAtPeriodEnd { get; set; } = true;
        public string? CancellationReason { get; set; }
    }

    /// <summary>
    /// ES: DTO para actualizar suscripción
    /// EN: DTO for updating subscription
    /// </summary>
    public class UpdateSubscriptionDto
    {
        public int SubscriptionId { get; set; }
        public int NewSubscriptionTypeId { get; set; }
        public bool IsAnnual { get; set; } = false;
        public bool ProrationBehavior { get; set; } = true;
    }
}
