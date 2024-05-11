using AutoMapper;
using Infrastructure.BasketReposatory;
using Infrastructure.BasketReposatory.BasketEntities;
using Services.BasketServices.Interfaces;
using Services.BasketServices.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BasketServices.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketReposatory _Basketreposatory;
        private readonly IMapper _Mapper;

        public BasketService(IBasketReposatory Basketreposatory, IMapper mapper)
        {
            _Basketreposatory = Basketreposatory;
            _Mapper = mapper;
        }
        public async Task<bool> DeleteCustomerBasket(string BasketId)
            => await _Basketreposatory.DeleteBasketAsync(BasketId);

        public async Task<CustomerBasketDto> GetCustomerBasket(string BasketId)
        {
            var customerBasket = await _Basketreposatory.GetBasketAsync(BasketId);
            if(customerBasket == null)
                return new CustomerBasketDto();
            var mappedCustomerBasket = _Mapper.Map<CustomerBasketDto>(customerBasket);

            return mappedCustomerBasket;
        }

        public async Task<CustomerBasketDto> UpdateCustomerBasket(CustomerBasketDto Basket)
        {
            var basket = _Mapper.Map<CustomerBasket>(Basket);
            var updateCustomerBasket = await _Basketreposatory.UpdateBasketAsync(basket);
            var mappedCustomerBasket = _Mapper.Map<CustomerBasketDto>(updateCustomerBasket);

            return mappedCustomerBasket;
        }
    }
}
