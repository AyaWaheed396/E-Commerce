using Admin_Dashboard.Models;
using AutoMapper;
using Core.Entities;

namespace Admin_Dashboard.Helpers
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();    
        }
    }
}
