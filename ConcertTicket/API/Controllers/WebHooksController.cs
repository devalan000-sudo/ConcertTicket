using ConcertTicket.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ConcertTicket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebHooksController : ControllerBase
    {
        private readonly string _webhookSecret;
        private readonly IReservationService _reservationService;

        public WebHooksController(IConfiguration configuration, IReservationService reservationService)
        {
            _webhookSecret = configuration["Stripe:WebhookSecret"] ?? throw new ArgumentNullException("Webhook secret is missing");
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> StripeWebhook()
        {
            //Stripe requiere que leamos el cuerpo crudo de la peticion
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                //Va;oda,ps qie el mensaje realmente venga de Stripe
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"],
                    _webhookSecret);
                
                //Escuchamos especificamente el evento de peso exitoso
                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    //Extraemos el ID de la reserva que guardamos en los metadatos en el paso anterior
                    if (paymentIntent != null && paymentIntent.Metadata.TryGetValue("ReservationId", out string reservationIdStr))
                    {
                        var reservationId = Guid.Parse(reservationIdStr);
                        //Pago confirmado, actualizamos la base de datos
                        await _reservationService.ConfirmPaymentAsync(reservationId);
                    }

                }   //Siempre debemos de devolver un 200 Ok rapido para que Stripe sepa que lo recibimos
                return Ok();
            }
            catch (StripeException)
            {
                //Si la firma no coincide, devolvemos un 400
                return BadRequest();
            }
        }
    }
}
