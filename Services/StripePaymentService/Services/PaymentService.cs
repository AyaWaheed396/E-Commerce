using AutoMapper;
using Core.Entities;
using Core.Entities.OrderEntities;
using Infrastructure.Interfaces;
using Infrastructure.Specification;
using Microsoft.Extensions.Configuration;
using Services.BasketServices.Interfaces;
using Services.BasketServices.Services.Dto;
using Services.OrderService.Services.Dto;
using Services.StripePaymentService.Interface;
using Stripe;
using Product = Core.Entities.Product;

namespace Services.StripePaymentService.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IBasketService _BasketService;
        private readonly IConfiguration _Configuration;
        private readonly IMapper _Mapper;

        public PaymentService(
            IUnitOfWork unitOfWork,
            IBasketService basketService,
            IConfiguration configuration,
            IMapper mapper
            )
        {
            _UnitOfWork = unitOfWork;
            _BasketService = basketService;
            _Configuration = configuration;
            _Mapper = mapper;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _Configuration["Stripe:SecretKey"];

            var basket = await _BasketService.GetCustomerBasket(basketId);
            if (basket == null)
                return null;
            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _UnitOfWork.Reposatory<Delivery>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _UnitOfWork.Reposatory<Product>().GetByIdAsync(item.Id);

                if(item.Price != productItem.Price)
                    item.Price = productItem.Price;

            }

            var service = new PaymentIntentService();
            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(i => i.Qty * (i.Price * 100)) + ((long)shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(i => i.Qty * (i.Price * 100)) + ((long)shippingPrice * 100),
                    
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _BasketService.UpdateCustomerBasket(basket);

            return basket;

        }

        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(CustomerBasketDto basket)
        {
            StripeConfiguration.ApiKey = _Configuration["Stripe:SecretKey"];
            if (basket == null)
                return null;
            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _UnitOfWork.Reposatory<Delivery>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _UnitOfWork.Reposatory<Product>().GetByIdAsync(item.Id);

                if (item.Price != productItem.Price)
                    item.Price = productItem.Price;

            }

            var service = new PaymentIntentService();
            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(i => i.Qty * (i.Price * 100)) + ((long)shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(i => i.Qty * (i.Price * 100)) + ((long)shippingPrice * 100),

                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _BasketService.UpdateCustomerBasket(basket);

            return basket;

        }

        public async Task<OrderResultDto> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var specs = new OrderWIthPaymentIntentSpecification(paymentIntentId);
            var order = await _UnitOfWork.Reposatory<Order>().GetEntityWithSpecificationsAsync(specs);
            if (order == null)
                return null;
            order.OrderStatus = OrderStatus.PaymentFailed;

            _UnitOfWork.Reposatory<Order>().Update(order);

            await _UnitOfWork.Complete();

            var mappedOrder = _Mapper.Map<OrderResultDto>(order);
            return mappedOrder;

        }

        public async Task<OrderResultDto> UpdateOrderPaymentSecceeded(string paymentIntentId, string basketId)
        {
            var specs = new OrderWIthPaymentIntentSpecification(paymentIntentId);
            var order = await _UnitOfWork.Reposatory<Order>().GetEntityWithSpecificationsAsync(specs);
            if (order == null)
                return null;
            order.OrderStatus = OrderStatus.PaymentRecived;

            _UnitOfWork.Reposatory<Order>().Update(order);

            await _UnitOfWork.Complete();

            await _BasketService.DeleteCustomerBasket(basketId);

            var mappedOrder = _Mapper.Map<OrderResultDto>(order);
            return mappedOrder;
        }
    }
}
