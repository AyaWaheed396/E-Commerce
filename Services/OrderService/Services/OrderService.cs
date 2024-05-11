using AutoMapper;
using Core.Entities;
using Core.Entities.OrderEntities;
using Infrastructure.Interfaces;
using Infrastructure.Specification;
using Services.BasketServices.Interfaces;
using Services.BasketServices.Services.Dto;
using Services.OrderService.Interface;
using Services.OrderService.Services.Dto;
using Services.StripePaymentService.Interface;
using Product = Core.Entities.Product;


namespace Services.OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _BasketService;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IPaymentService _PaymentService;
        private readonly IMapper _Mapper;

        public OrderService(
            IBasketService basketService,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService,
            IMapper mapper)
        {
            _BasketService = basketService;
            _UnitOfWork = unitOfWork;
            _PaymentService = paymentService;
            _Mapper = mapper;
        }
        public async Task<OrderResultDto> CreateOrder(OrderDto orderDto)
        {
            //Get Basket
            var basket = await _BasketService.GetCustomerBasket(orderDto.BasketId);
            if (basket == null)
                return null;

            var orderItems = await MappOrderItem(basket.BasketItems);
            //Get Delivery Method

            var deliveryMethod = await _UnitOfWork.Reposatory<Delivery>().GetByIdAsync(orderDto.DeliveryMethodId);

            // Calculate SubTotal

            var subTotal = orderItems.Sum(i => i.Price * i.Quantity);

            //Check if order Exist

            var specs = new OrderWIthPaymentIntentSpecification(basket.PaymentIntentId);

            var existingOrder = await _UnitOfWork.Reposatory<Order>().GetEntityWithSpecificationsAsync(specs);

            CustomerBasketDto customerBasket = new CustomerBasketDto();

            if(existingOrder != null)
            {
                _UnitOfWork.Reposatory<Order>().Delete(existingOrder);
                await _PaymentService.CreateOrUpdatePaymentIntent(basket);
            }
            else
                customerBasket  = await _PaymentService.CreateOrUpdatePaymentIntent(basket.Id);

            // create order

            var mappedOrderItems = _Mapper.Map<List<OrderItem>>(orderItems);

            var mappedShippingAddress = _Mapper.Map<ShippingAddress>(orderDto.ShippingAddress);



            var order = new Order(orderDto.BuyerEmail, mappedShippingAddress, deliveryMethod, mappedOrderItems, subTotal, basket.PaymentIntentId ?? customerBasket.PaymentIntentId);

            await _UnitOfWork.Reposatory<Order>().Add(order);

            await _UnitOfWork.Complete();

            var mappedOrder = _Mapper.Map<OrderResultDto>(order);

            return mappedOrder;

        }

        private async Task<List<OrderItemDto>> MappOrderItem(List<BasketItemDto> basketItems)
        {
            var orderItems = new List<OrderItemDto>();

            foreach (var item in basketItems)
            {
                var productItem = await _UnitOfWork.Reposatory<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(productItem.Price, item.Qty, itemOrdered);
                var mappedOrderItem = _Mapper.Map<OrderItemDto>(orderItem);
                orderItems.Add(mappedOrderItem);
            }
            return orderItems;

        }

        public async Task<IReadOnlyList<Delivery>> GetAllDeliveryMethodAsync()
            => await _UnitOfWork.Reposatory<Delivery>().GetAllAsync();

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrderForUserAsync(string byerEmail)
        {
            var specs = new OrderWithItemsSpecification(byerEmail);
            var orders = await _UnitOfWork.Reposatory<Order>().GetAllWithSpecificationsAsync(specs);
            var mappedOrder = _Mapper.Map<List<OrderResultDto>>(orders);

            return mappedOrder;
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var specs = new OrderWithItemsSpecification(id, buyerEmail);
            var order = await _UnitOfWork.Reposatory<Order>().GetEntityWithSpecificationsAsync(specs);
            var mappedOrder = _Mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }
    }
}
