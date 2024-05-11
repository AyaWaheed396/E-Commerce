using AutoMapper;
using Core.Entities;
using Services.ProductServices.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Dto
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResultDto>()
                .ForMember(pd => pd.ProductBrandName,options => options.MapFrom(p => p.ProductBrand.Name))
                .ForMember(pd => pd.ProductTypeName,options => options.MapFrom(p => p.ProductType.Name))
                .ForMember(pd=>pd.PictureUrl, options => options.MapFrom<ProductUrlResolver>());

        }
    }
}
