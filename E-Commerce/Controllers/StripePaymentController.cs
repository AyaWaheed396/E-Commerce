using E_Commerce.HandelResponses;
using Microsoft.AspNetCore.Mvc;
using Services.BasketServices.Services.Dto;
using Services.OrderService.Services.Dto;
using Services.StripePaymentService.Interface;
using Stripe;

namespace E_Commerce.Controllers
{

    public class StripePaymentController : BaseController
    {
        private readonly IPaymentService _PaymentService;
        private readonly ILogger<StripePaymentController> _Logger;
        private const string WHSecret = "whsec_ebd3cf3b1b850294315daaa940be44f1eaf4f852c4fe6805a971dc293479f528";

        public StripePaymentController(
            IPaymentService paymentService,
            ILogger<StripePaymentController> logger)
        {
            _PaymentService = paymentService;
            _Logger = logger;
        }
        [HttpPost("{basket}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto basket)
        {
            var customerBasket = await _PaymentService.CreateOrUpdatePaymentIntent(basket);
            if (customerBasket == null)
                return BadRequest(new ApiResponse(400, "Problem With Your Basket"));

            return Ok(customerBasket);
        }

        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForNewOrder(string basketId)
        {
            var customerBasket = await _PaymentService.CreateOrUpdatePaymentIntent(basketId);
            if (customerBasket == null)
                return BadRequest(new ApiResponse(400, "Problem With Your Basket"));

            return Ok(customerBasket);
        }

        [HttpPost]
        public async Task<ActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,Request.Headers["Stripe-Signature"], WHSecret);

                PaymentIntent paymentIntent;
                OrderResultDto order;

                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _Logger.LogInformation("Payment Failed: ", paymentIntent.Id);

                    order = await _PaymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    _Logger.LogInformation("Order Updated To Payment Failed: ", order.BuyerEmail);

                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _Logger.LogInformation("Payment Succeeded: ", paymentIntent.Id);

                    order = await _PaymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    _Logger.LogInformation("Order Updated To Payment Succeeded: ", order.BuyerEmail);

                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

    }
}
