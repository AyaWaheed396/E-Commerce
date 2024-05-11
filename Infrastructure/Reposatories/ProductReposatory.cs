using Core.Context;
using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Reposatories
{
    public class ProductReposatory : IProductReposatory
    {
        private readonly StoreDbContext _Context;

        public ProductReposatory(StoreDbContext context)
        {
            _Context = context;
        }

        public async Task<IReadOnlyList<Product>> GetAllProductAsync()
            => await _Context.Products.ToListAsync();

        public async Task<IReadOnlyList<ProductBrand>> GetAllProductBrandsAsync()
            => await _Context.ProductBrands.ToListAsync();

        public async Task<IReadOnlyList<ProductType>> GetAllProductTypesAsync()
            => await _Context.ProductTypes.ToListAsync();

        public async Task<Product> GetProductByIdASync(int? id)
            => await _Context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }
}
