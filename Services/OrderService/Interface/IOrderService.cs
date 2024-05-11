using Core.Entities;
using Services.OrderService.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderService.Interface
{
    public interface IOrderService
    {
        Task<OrderResultDto> CreateOrder(OrderDto orderDto);

        Task<IReadOnlyList<OrderResultDto>> GetAllOrderForUserAsync(string buyerEmail);

        Task<OrderResultDto> GetOrderByIdAsync(int id, string buyerEmail);

        Task<IReadOnlyList<Delivery>> GetAllDeliveryMethodAsync();


    }
}
