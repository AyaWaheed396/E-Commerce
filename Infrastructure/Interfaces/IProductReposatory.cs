using Core.Entities;
using Infrastructure.Reposatories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IProductReposatory
    {
        Task<IReadOnlyList<Product>> GetAllProductAsync();
        Task<Product> GetProductByIdASync(int? id);
        Task<IReadOnlyList<ProductBrand>> GetAllProductBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetAllProductTypesAsync();



    }
}
