using Services.BasketServices.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BasketServices.Interfaces
{
    public interface IBasketService
    {
        Task<CustomerBasketDto> GetCustomerBasket(string BasketId);
        Task<CustomerBasketDto> UpdateCustomerBasket(CustomerBasketDto Basket);

        Task<bool> DeleteCustomerBasket(string BasketId);

    }
}
