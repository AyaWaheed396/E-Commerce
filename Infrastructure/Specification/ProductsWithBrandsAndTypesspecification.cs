using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specification
{
    public class ProductsWithBrandsAndTypesspecification : BaseSpecification<Product>
    {
        public ProductsWithBrandsAndTypesspecification(ProductSpecification specification) 
            : base(s => 
                (!specification.BrandId.HasValue || s.ProductBrandId == specification.BrandId)
            &&
                (!specification.TypeId.HasValue || s.ProductTypeId == specification.TypeId)
            &&
                (string.IsNullOrEmpty(specification.SearchName) || s.Name.Trim().ToLower().Contains(specification.SearchName))
                  )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            AddOrderBy(p => p.Name);

            ApplyPagination(specification.PageSize*(specification.PageIndex-1), specification.PageSize);

            if (!string.IsNullOrEmpty(specification.sort))
            {
                switch (specification.sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price); 
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;

                    default:
                        AddOrderBy(p=>p.Name); 
                        break;

                }

            }

        }
        public ProductsWithBrandsAndTypesspecification(int? id)
            : base(s => s.Id==id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);


        }
    }
}
