using BocciaCoaching.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("SubscriptionType")]
    public class SubscriptionType : IAuditable
    {
        /// <summary>
        /// ES: Identificador del tipo de suscripción
        /// EN: Subscription type identifier
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SubscriptionTypeId { get; set; }

        /// <summary>
        /// ES: Nombre del tipo de suscripción (Free, Premium, etc.)
        /// EN: Subscription type name (Free, Premium, etc.)
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// ES: Descripción del tipo de suscripción
        /// EN: Subscription type description
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// ES: Precio mensual en centavos (ej: 999 = $9.99)
        /// EN: Monthly price in cents (ex: 999 = $9.99)
        /// </summary>
        public int PriceInCents { get; set; }

        /// <summary>
        /// ES: Precio anual en centavos
        /// EN: Annual price in cents
        /// </summary>
        public int? AnnualPriceInCents { get; set; }

        /// <summary>
        /// ES: ID del producto en Stripe
        /// EN: Stripe product ID
        /// </summary>
        public string? StripeProductId { get; set; }

        /// <summary>
        /// ES: ID del precio mensual en Stripe
        /// EN: Stripe monthly price ID
        /// </summary>
        public string? StripeMonthlyPriceId { get; set; }

        /// <summary>
        /// ES: ID del precio anual en Stripe
        /// EN: Stripe annual price ID
        /// </summary>
        public string? StripeAnnualPriceId { get; set; }

        /// <summary>
        /// ES: Características incluidas en JSON
        /// EN: Features included in JSON
        /// </summary>
        public string? Features { get; set; }

        /// <summary>
        /// ES: Límite de equipos que puede crear
        /// EN: Team creation limit
        /// </summary>
        public int? TeamLimit { get; set; }

        /// <summary>
        /// ES: Límite de atletas por equipo
        /// EN: Athletes per team limit
        /// </summary>
        public int? AthleteLimit { get; set; }

        /// <summary>
        /// ES: Límite de evaluaciones mensuales
        /// EN: Monthly evaluations limit
        /// </summary>
        public int? MonthlyEvaluationLimit { get; set; }

        /// <summary>
        /// ES: Acceso a estadísticas avanzadas
        /// EN: Advanced statistics access
        /// </summary>
        public bool HasAdvancedStatistics { get; set; } = false;

        /// <summary>
        /// ES: Acceso a chat premium
        /// EN: Premium chat access
        /// </summary>
        public bool HasPremiumChat { get; set; } = false;

        /// <summary>
        /// ES: Estado activo del tipo de suscripción
        /// EN: Active status of subscription type
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// ES: Es el plan gratuito por defecto
        /// EN: Is the default free plan
        /// </summary>
        public bool IsDefault { get; set; } = false;

        // Navigation properties
        public ICollection<Subscription>? Subscriptions { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
