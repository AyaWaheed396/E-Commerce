using AutoMapper;
using Infrastructure.BasketReposatory.BasketEntities;

namespace Services.BasketServices.Services.Dto
{
    public class BasketProfile : Profile
    {
        public BasketProfile() 
        {
            CreateMap<CustomerBasketDto,CustomerBasket>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();


        }
    }
}
