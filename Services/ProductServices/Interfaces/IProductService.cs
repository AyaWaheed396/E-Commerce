using Core.Entities;
using Infrastructure.Specification;
using Services.Hepler;
using Services.Services.Dto;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<Pagination<ProductResultDto>> GetAllProductAsync(ProductSpecification specification);
        Task<ProductResultDto> GetProductByIdAsync(int? id);
        Task<IReadOnlyList<ProductBrand>> GetAllProductBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetAllProductTypesAsync();


    }
}
