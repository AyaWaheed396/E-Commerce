using Core.Entities;
using E_Commerce.HandelResponses;
using E_Commerce.Helper;
using Infrastructure.Specification;
using Microsoft.AspNetCore.Mvc;
using Services.Hepler;
using Services.Interfaces;
using Services.Services.Dto;

namespace E_Commerce.Controllers
{

    public class ProductController : BaseController
    {
        
        private readonly IProductService _ProductService;

        public ProductController(IProductService productService)
        {
            _ProductService = productService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        //[Cashe(100)]

        public async Task<ActionResult<Pagination<ProductResultDto>>> GetProducts([FromQuery] ProductSpecification specification)
        {
            var product = await _ProductService.GetAllProductAsync(specification);

            return Ok(product);
        }
        [HttpGet]

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
            => Ok(await _ProductService.GetAllProductBrandsAsync());
        [HttpGet]

        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
            => Ok(await _ProductService.GetAllProductTypesAsync());
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [Cashe(100)]

        public async Task<ActionResult<ProductResultDto>> GetProductById(int? id)
        {
            var product = await _ProductService.GetProductByIdAsync(id);

            if (product is null)
                return NotFound(new ApiResponse(404));

            return Ok(product);
        }



    }
}
