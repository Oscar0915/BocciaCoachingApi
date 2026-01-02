using BocciaCoaching.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BocciaCoaching.Models.Entities
{
    [Table("Payment")]
    public class Payment : IAuditable
    {
        /// <summary>
        /// ES: Identificador del pago
        /// EN: Payment identifier
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int PaymentId { get; set; }

        /// <summary>
        /// ES: ID de la suscripción
        /// EN: Subscription ID
        /// </summary>
        [ForeignKey("Subscription")]
        public int SubscriptionId { get; set; }

        /// <summary>
        /// ES: ID del usuario
        /// EN: User ID
        /// </summary>
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// ES: ID del pago en Stripe
        /// EN: Stripe payment intent ID
        /// </summary>
        public string? StripePaymentIntentId { get; set; }

        /// <summary>
        /// ES: ID de la factura en Stripe
        /// EN: Stripe invoice ID
        /// </summary>
        public string? StripeInvoiceId { get; set; }

        /// <summary>
        /// ES: Cantidad pagada en centavos
        /// EN: Amount paid in cents
        /// </summary>
        public int AmountInCents { get; set; }

        /// <summary>
        /// ES: Moneda del pago
        /// EN: Payment currency
        /// </summary>
        [Required]
        [MaxLength(3)]
        public string Currency { get; set; } = "USD";

        /// <summary>
        /// ES: Estado del pago
        /// EN: Payment status
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "pending"; // pending, succeeded, failed, canceled, refunded

        /// <summary>
        /// ES: Método de pago
        /// EN: Payment method
        /// </summary>
        [MaxLength(20)]
        public string? PaymentMethod { get; set; } = "card"; // card, bank_transfer, etc.

        /// <summary>
        /// ES: Descripción del pago
        /// EN: Payment description
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// ES: Fecha del pago
        /// EN: Payment date
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// ES: Fecha de procesamiento
        /// EN: Processing date
        /// </summary>
        public DateTime? ProcessedAt { get; set; }

        /// <summary>
        /// ES: Fecha de reembolso
        /// EN: Refund date
        /// </summary>
        public DateTime? RefundedAt { get; set; }

        /// <summary>
        /// ES: Cantidad reembolsada en centavos
        /// EN: Refunded amount in cents
        /// </summary>
        public int? RefundedAmountInCents { get; set; }

        /// <summary>
        /// ES: Motivo del fallo (si aplica)
        /// EN: Failure reason (if applicable)
        /// </summary>
        public string? FailureReason { get; set; }

        /// <summary>
        /// ES: Código de fallo
        /// EN: Failure code
        /// </summary>
        public string? FailureCode { get; set; }

        /// <summary>
        /// ES: Información adicional del pago
        /// EN: Additional payment information
        /// </summary>
        public string? Metadata { get; set; }

        /// <summary>
        /// ES: Número de recibo
        /// EN: Receipt number
        /// </summary>
        public string? ReceiptNumber { get; set; }

        // Navigation properties
        public Subscription? Subscription { get; set; }
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
