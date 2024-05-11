using Core.Entities;
using E_Commerce.HandelResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.OrderService.Interface;
using Services.OrderService.Services.Dto;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _OrderService;

        public OrdersController( IOrderService orderService)
        {
            _OrderService = orderService;
        }
        [HttpPost]
        public async Task<ActionResult<OrderResultDto>> CreateOrUpdateOrder(OrderDto orderDto)
        {
            var order = await _OrderService.CreateOrder(orderDto);
            if (order == null)
                return BadRequest(new ApiResponse(400, "Error While Creating New Order"));
            return Ok(order);

        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderResultDto>>> GetAllOrderForUserAsync()
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var orders = await _OrderService.GetAllOrderForUserAsync(email);
            if (orders.Count <= 0)
                return Ok(new ApiResponse(200,"You Don't Have Any Orders Yet"));

            return Ok(orders);
        }
        [HttpGet]
        public async Task<ActionResult<OrderResultDto>> GetOrderByIdAsync(int id)
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);

            var order = await _OrderService.GetOrderByIdAsync(id, email);

            if (order == null)
                return Ok(new ApiResponse(200, $"There is no Order WIth id :{id}"));
            return Ok(order);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Delivery>>> GetAllDeliveryMethodAsync()
            => Ok(await _OrderService.GetAllDeliveryMethodAsync());
    }
}
