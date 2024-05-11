using E_Commerce.HandelResponses;
using Microsoft.AspNetCore.Mvc;
using Services.BasketServices.Interfaces;
using Services.BasketServices.Services.Dto;

namespace E_Commerce.Controllers
{

    public class BasketController : BaseController
    {
        private readonly IBasketService _BasketService;

        public BasketController(IBasketService basketService)
        {
            _BasketService = basketService;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerBasketDto>> GetBasket(string id)
        {
            var basket = await _BasketService.GetCustomerBasket(id);
            if(basket.Id == null)
                return NotFound(new ApiResponse(404));
            return Ok(basket);
            
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdateBasket(CustomerBasketDto Basket)
        {
            var basket = await _BasketService.UpdateCustomerBasket(Basket);
            if (basket == null)
                return NotFound(new ApiResponse(404));
            return Ok(basket);

        }
        [HttpDelete]
        public async Task DeleteBasket(string Id)
            => Ok(await _BasketService.DeleteCustomerBasket(Id));
    }
}
