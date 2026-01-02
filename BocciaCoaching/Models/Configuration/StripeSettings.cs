namespace BocciaCoaching.Models.Configuration
{
    /// <summary>
    /// ES: Configuración para Stripe
    /// EN: Stripe configuration
    /// </summary>
    public class StripeSettings
    {
        /// <summary>
        /// ES: Clave pública de Stripe
        /// EN: Stripe publishable key
        /// </summary>
        public string PublishableKey { get; set; } = string.Empty;

        /// <summary>
        /// ES: Clave secreta de Stripe
        /// EN: Stripe secret key
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// ES: Secreto del webhook
        /// EN: Webhook secret
        /// </summary>
        public string WebhookSecret { get; set; } = string.Empty;

        /// <summary>
        /// ES: Moneda por defecto
        /// EN: Default currency
        /// </summary>
        public string Currency { get; set; } = "USD";
    }
}
