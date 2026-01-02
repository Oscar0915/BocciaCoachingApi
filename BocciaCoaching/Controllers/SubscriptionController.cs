using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Subscription;
using BocciaCoaching.Models.DTO.Payment;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    /// <summary>
    /// ES: Controlador para manejo de suscripciones
    /// EN: Controller for subscription management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IStripePaymentService _stripePaymentService;

        public SubscriptionController(ISubscriptionService subscriptionService, IStripePaymentService stripePaymentService)
        {
            _subscriptionService = subscriptionService;
            _stripePaymentService = stripePaymentService;
        }

        #region Subscription Types

        /// <summary>
        /// ES: Obtiene todos los tipos de suscripción disponibles
        /// EN: Gets all available subscription types
        /// </summary>
        [HttpGet("types")]
        public async Task<ActionResult<ResponseContract<IEnumerable<SubscriptionTypeDto>>>> GetSubscriptionTypes()
        {
            var response = await _subscriptionService.GetAllSubscriptionTypesAsync();
            return Ok(response);
        }

        /// <summary>
        /// ES: Obtiene un tipo de suscripción por ID
        /// EN: Gets a subscription type by ID
        /// </summary>
        [HttpGet("types/{id}")]
        public async Task<ActionResult<ResponseContract<SubscriptionTypeDto>>> GetSubscriptionType(int id)
        {
            var response = await _subscriptionService.GetSubscriptionTypeByIdAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// ES: Crea un nuevo tipo de suscripción (Solo para administradores)
        /// EN: Creates a new subscription type (Admin only)
        /// </summary>
        [HttpPost("types")]
        public async Task<ActionResult<ResponseContract<SubscriptionTypeDto>>> CreateSubscriptionType(CreateSubscriptionTypeDto createDto)
        {
            var response = await _subscriptionService.CreateSubscriptionTypeAsync(createDto);
            return Ok(response);
        }

        /// <summary>
        /// ES: Actualiza un tipo de suscripción (Solo para administradores)
        /// EN: Updates a subscription type (Admin only)
        /// </summary>
        [HttpPut("types/{id}")]
        public async Task<ActionResult<ResponseContract<SubscriptionTypeDto>>> UpdateSubscriptionType(int id, CreateSubscriptionTypeDto updateDto)
        {
            var response = await _subscriptionService.UpdateSubscriptionTypeAsync(id, updateDto);
            return Ok(response);
        }

        /// <summary>
        /// ES: Elimina un tipo de suscripción (Solo para administradores)
        /// EN: Deletes a subscription type (Admin only)
        /// </summary>
        [HttpDelete("types/{id}")]
        public async Task<ActionResult<ResponseContract<bool>>> DeleteSubscriptionType(int id)
        {
            var response = await _subscriptionService.DeleteSubscriptionTypeAsync(id);
            return Ok(response);
        }

        #endregion

        #region User Subscriptions

        /// <summary>
        /// ES: Obtiene la suscripción actual del usuario
        /// EN: Gets user's current subscription
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ResponseContract<UserSubscriptionDto>>> GetUserSubscription(int userId)
        {
            var response = await _subscriptionService.GetUserSubscriptionAsync(userId);
            return Ok(response);
        }

        /// <summary>
        /// ES: Obtiene el historial de suscripciones del usuario
        /// EN: Gets user's subscription history
        /// </summary>
        [HttpGet("user/{userId}/history")]
        public async Task<ActionResult<ResponseContract<IEnumerable<UserSubscriptionDto>>>> GetUserSubscriptionHistory(int userId)
        {
            var response = await _subscriptionService.GetUserSubscriptionHistoryAsync(userId);
            return Ok(response);
        }

        /// <summary>
        /// ES: Crea una nueva suscripción para el usuario
        /// EN: Creates a new subscription for the user
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult<ResponseContract<SubscriptionCreateResponseDto>>> CreateSubscription(CreateSubscriptionDto createDto)
        {
            var response = await _subscriptionService.CreateSubscriptionAsync(createDto);
            return Ok(response);
        }

        /// <summary>
        /// ES: Cancela una suscripción
        /// EN: Cancels a subscription
        /// </summary>
        [HttpPost("cancel")]
        public async Task<ActionResult<ResponseContract<bool>>> CancelSubscription(CancelSubscriptionDto cancelDto)
        {
            var response = await _subscriptionService.CancelSubscriptionAsync(cancelDto);
            return Ok(response);
        }

        /// <summary>
        /// ES: Actualiza una suscripción (cambio de plan)
        /// EN: Updates a subscription (plan change)
        /// </summary>
        [HttpPut("update")]
        public async Task<ActionResult<ResponseContract<UserSubscriptionDto>>> UpdateSubscription(UpdateSubscriptionDto updateDto)
        {
            var response = await _subscriptionService.UpdateSubscriptionAsync(updateDto);
            return Ok(response);
        }

        /// <summary>
        /// ES: Reactiva una suscripción cancelada
        /// EN: Reactivates a cancelled subscription
        /// </summary>
        [HttpPost("reactivate/{subscriptionId}")]
        public async Task<ActionResult<ResponseContract<UserSubscriptionDto>>> ReactivateSubscription(int subscriptionId)
        {
            var response = await _subscriptionService.ReactivateSubscriptionAsync(subscriptionId);
            return Ok(response);
        }

        #endregion

        #region Trial Management

        /// <summary>
        /// ES: Inicia un período de prueba
        /// EN: Starts a trial period
        /// </summary>
        [HttpPost("trial/start")]
        public async Task<ActionResult<ResponseContract<UserSubscriptionDto>>> StartTrial(int userId, int subscriptionTypeId, int trialDays = 7)
        {
            var response = await _subscriptionService.StartTrialAsync(userId, subscriptionTypeId, trialDays);
            return Ok(response);
        }

        /// <summary>
        /// ES: Verifica si el usuario puede usar el período de prueba
        /// EN: Checks if user can use trial period
        /// </summary>
        [HttpGet("trial/available/{userId}/{subscriptionTypeId}")]
        public async Task<ActionResult<ResponseContract<bool>>> IsTrialAvailable(int userId, int subscriptionTypeId)
        {
            var response = await _subscriptionService.IsTrialAvailableAsync(userId, subscriptionTypeId);
            return Ok(response);
        }

        #endregion

        #region Subscription Validation

        /// <summary>
        /// ES: Valida si el usuario tiene una suscripción activa
        /// EN: Validates if user has an active subscription
        /// </summary>
        [HttpGet("validate/{userId}")]
        public async Task<ActionResult<ResponseContract<bool>>> ValidateUserSubscription(int userId, [FromQuery] string? feature = null)
        {
            ResponseContract<bool> response;
            
            if (!string.IsNullOrEmpty(feature))
            {
                response = await _subscriptionService.ValidateUserSubscriptionAsync(userId, feature);
            }
            else
            {
                response = await _subscriptionService.HasActiveSubscriptionAsync(userId);
            }
            
            return Ok(response);
        }

        /// <summary>
        /// ES: Verifica si el usuario puede acceder a una característica específica
        /// EN: Checks if user can access a specific feature
        /// </summary>
        [HttpGet("access/{userId}/{featureName}")]
        public async Task<ActionResult<ResponseContract<bool>>> CanAccessFeature(int userId, string featureName)
        {
            var response = await _subscriptionService.CanAccessFeatureAsync(userId, featureName);
            return Ok(response);
        }

        #endregion

        #region Subscription Limits

        /// <summary>
        /// ES: Verifica si el usuario puede crear un equipo
        /// EN: Checks if user can create a team
        /// </summary>
        [HttpGet("limits/{userId}/can-create-team")]
        public async Task<ActionResult<ResponseContract<bool>>> CanCreateTeam(int userId)
        {
            var response = await _subscriptionService.CanCreateTeamAsync(userId);
            return Ok(response);
        }

        /// <summary>
        /// ES: Verifica si el usuario puede agregar un atleta al equipo
        /// EN: Checks if user can add athlete to team
        /// </summary>
        [HttpGet("limits/{userId}/can-add-athlete/{teamId}")]
        public async Task<ActionResult<ResponseContract<bool>>> CanAddAthlete(int userId, int teamId)
        {
            var response = await _subscriptionService.CanAddAthleteAsync(userId, teamId);
            return Ok(response);
        }

        /// <summary>
        /// ES: Verifica si el usuario puede realizar una evaluación
        /// EN: Checks if user can perform an evaluation
        /// </summary>
        [HttpGet("limits/{userId}/can-evaluate")]
        public async Task<ActionResult<ResponseContract<bool>>> CanPerformEvaluation(int userId)
        {
            var response = await _subscriptionService.CanPerformEvaluationAsync(userId);
            return Ok(response);
        }

        /// <summary>
        /// ES: Obtiene los equipos restantes que puede crear el usuario
        /// EN: Gets remaining teams user can create
        /// </summary>
        [HttpGet("limits/{userId}/remaining-teams")]
        public async Task<ActionResult<ResponseContract<int>>> GetRemainingTeams(int userId)
        {
            var response = await _subscriptionService.GetRemainingTeamsAsync(userId);
            return Ok(response);
        }

        /// <summary>
        /// ES: Obtiene los atletas restantes que puede agregar al equipo
        /// EN: Gets remaining athletes that can be added to team
        /// </summary>
        [HttpGet("limits/{userId}/remaining-athletes/{teamId}")]
        public async Task<ActionResult<ResponseContract<int>>> GetRemainingAthletes(int userId, int teamId)
        {
            var response = await _subscriptionService.GetRemainingAthletesAsync(userId, teamId);
            return Ok(response);
        }

        /// <summary>
        /// ES: Obtiene las evaluaciones restantes del mes
        /// EN: Gets remaining evaluations for the month
        /// </summary>
        [HttpGet("limits/{userId}/remaining-evaluations")]
        public async Task<ActionResult<ResponseContract<int>>> GetRemainingEvaluations(int userId)
        {
            var response = await _subscriptionService.GetRemainingEvaluationsAsync(userId);
            return Ok(response);
        }

        #endregion

        #region Payment Integration

        /// <summary>
        /// ES: Crea una intención de pago
        /// EN: Creates a payment intent
        /// </summary>
        [HttpPost("payment/create-intent")]
        public async Task<ActionResult<ResponseContract<PaymentIntentResponseDto>>> CreatePaymentIntent(CreatePaymentIntentDto createDto)
        {
            var response = await _stripePaymentService.CreatePaymentIntentAsync(createDto);
            return Ok(response);
        }

        /// <summary>
        /// ES: Confirma un pago
        /// EN: Confirms a payment
        /// </summary>
        [HttpPost("payment/confirm")]
        public async Task<ActionResult<ResponseContract<PaymentIntentResponseDto>>> ConfirmPayment(ConfirmPaymentDto confirmDto)
        {
            var response = await _stripePaymentService.ConfirmPaymentIntentAsync(confirmDto);
            return Ok(response);
        }

        /// <summary>
        /// ES: Obtiene información de un pago
        /// EN: Gets payment information
        /// </summary>
        [HttpGet("payment/{paymentIntentId}")]
        public async Task<ActionResult<ResponseContract<PaymentIntentResponseDto>>> GetPaymentIntent(string paymentIntentId)
        {
            var response = await _stripePaymentService.GetPaymentIntentAsync(paymentIntentId);
            return Ok(response);
        }

        /// <summary>
        /// ES: Cancela un pago pendiente
        /// EN: Cancels a pending payment
        /// </summary>
        [HttpPost("payment/cancel/{paymentIntentId}")]
        public async Task<ActionResult<ResponseContract<bool>>> CancelPaymentIntent(string paymentIntentId)
        {
            var response = await _stripePaymentService.CancelPaymentIntentAsync(paymentIntentId);
            return Ok(response);
        }

        #endregion

        #region Webhooks

        /// <summary>
        /// ES: Maneja webhooks de Stripe
        /// EN: Handles Stripe webhooks
        /// </summary>
        [HttpPost("webhooks/stripe")]
        public async Task<IActionResult> HandleStripeWebhook()
        {
            try
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var signature = Request.Headers["Stripe-Signature"];

                var eventResponse = await _stripePaymentService.ConstructWebhookEventAsync(json, signature!);
                if (!eventResponse.Success)
                {
                    return BadRequest(eventResponse.Message);
                }

                // Extract event type and data from the constructed event
                var stripeEvent = eventResponse.Data as dynamic;
                var eventType = stripeEvent?.Type?.ToString() ?? "";
                var eventData = stripeEvent?.Data;

                var response = await _subscriptionService.HandleStripeWebhookAsync(eventType, eventData);

                return Ok(new { received = true, processed = response.Success, message = response.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Admin Endpoints

        /// <summary>
        /// ES: Obtiene todas las suscripciones (Solo administradores)
        /// EN: Gets all subscriptions (Admin only)
        /// </summary>
        [HttpGet("admin/all")]
        public ActionResult<ResponseContract<IEnumerable<UserSubscriptionDto>>> GetAllSubscriptions()
        {
            // This would need to be implemented in the subscription service
            return Ok(ResponseContract<IEnumerable<UserSubscriptionDto>>.Ok(
                new List<UserSubscriptionDto>(),
                "Admin endpoint - not implemented yet"
            ));
        }

        /// <summary>
        /// ES: Obtiene estadísticas de suscripciones (Solo administradores)
        /// EN: Gets subscription statistics (Admin only)
        /// </summary>
        [HttpGet("admin/statistics")]
        public ActionResult<ResponseContract<object>> GetSubscriptionStatistics()
        {
            // This would need to be implemented in the subscription service
            return Ok(ResponseContract<object>.Ok(
                new { },
                "Admin endpoint - not implemented yet"
            ));
        }

        #endregion
    }
}
