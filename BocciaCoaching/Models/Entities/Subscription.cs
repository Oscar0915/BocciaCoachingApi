using BocciaCoaching.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Subscription")]
    public class Subscription : IAuditable
    {
        /// <summary>
        /// ES: Identificador de la suscripción
        /// EN: Subscription identifier
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SubscriptionId { get; set; }

        /// <summary>
        /// ES: ID del usuario
        /// EN: User ID
        /// </summary>
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// ES: ID del tipo de suscripción
        /// EN: Subscription type ID
        /// </summary>
        [ForeignKey("SubscriptionType")]
        public int SubscriptionTypeId { get; set; }

        /// <summary>
        /// ES: ID de la suscripción en Stripe
        /// EN: Stripe subscription ID
        /// </summary>
        public string? StripeSubscriptionId { get; set; }

        /// <summary>
        /// ES: ID del customer en Stripe
        /// EN: Stripe customer ID
        /// </summary>
        public string? StripeCustomerId { get; set; }

        /// <summary>
        /// ES: Estado de la suscripción
        /// EN: Subscription status
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "active"; // active, canceled, past_due, unpaid, incomplete

        /// <summary>
        /// ES: Fecha de inicio de la suscripción
        /// EN: Subscription start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// ES: Fecha de fin de la suscripción (null para suscripciones activas)
        /// EN: Subscription end date (null for active subscriptions)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// ES: Fecha de renovación
        /// EN: Next renewal date
        /// </summary>
        public DateTime? NextRenewalDate { get; set; }

        /// <summary>
        /// ES: Fecha de cancelación
        /// EN: Cancellation date
        /// </summary>
        public DateTime? CanceledAt { get; set; }

        /// <summary>
        /// ES: Si es un período de prueba
        /// EN: Is trial period
        /// </summary>
        public bool IsTrial { get; set; } = false;

        /// <summary>
        /// ES: Fecha de fin del período de prueba
        /// EN: Trial end date
        /// </summary>
        public DateTime? TrialEndDate { get; set; }

        /// <summary>
        /// ES: Si es una suscripción anual
        /// EN: Is annual subscription
        /// </summary>
        public bool IsAnnual { get; set; } = false;

        /// <summary>
        /// ES: Precio pagado en centavos
        /// EN: Price paid in cents
        /// </summary>
        public int? PricePaidInCents { get; set; }

        /// <summary>
        /// ES: Moneda utilizada
        /// EN: Currency used
        /// </summary>
        [MaxLength(3)]
        public string Currency { get; set; } = "USD";

        /// <summary>
        /// ES: Notas adicionales
        /// EN: Additional notes
        /// </summary>
        public string? Notes { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public SubscriptionType? SubscriptionType { get; set; }
        public ICollection<Payment>? Payments { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
