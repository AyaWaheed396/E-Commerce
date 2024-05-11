using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specification;
using Services.Hepler;
using Services.Interfaces;
using Services.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
            _Mapper = mapper;
        }
        public async Task<Pagination<ProductResultDto>> GetAllProductAsync(ProductSpecification specification)
        {
            var specs = new ProductsWithBrandsAndTypesspecification(specification);
            var product = await _UnitOfWork.Reposatory<Product>().GetAllWithSpecificationsAsync(specs);
            //var totalCount = await _UnitOfWork.Reposatory<Product>().CountAsync(specs);
            var mapped = _Mapper.Map<IReadOnlyList<ProductResultDto>>(product);
            
            return new Pagination<ProductResultDto>(specification.PageIndex,specification.PageSize, mapped.Count, mapped);
        }

        public async Task<IReadOnlyList<ProductBrand>> GetAllProductBrandsAsync()
            => await _UnitOfWork.Reposatory<ProductBrand>().GetAllAsync();

        public async Task<IReadOnlyList<ProductType>> GetAllProductTypesAsync() 
            => await _UnitOfWork.Reposatory<ProductType>().GetAllAsync();

        public async Task<ProductResultDto> GetProductByIdAsync(int? id)
        {
            var specs = new ProductsWithBrandsAndTypesspecification(id);
            var product = await _UnitOfWork.Reposatory<Product>().GetEntityWithSpecificationsAsync(specs);
            var mapped = _Mapper.Map<ProductResultDto>(product);
            return mapped;
        }
    }
}
