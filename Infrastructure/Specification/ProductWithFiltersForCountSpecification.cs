using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specification
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecification specification)
            : base(s =>
                (!specification.BrandId.HasValue || s.ProductBrandId == specification.BrandId)
            &&
                (!specification.TypeId.HasValue || s.ProductTypeId == specification.TypeId)
            &&
                (string.IsNullOrEmpty(specification.SearchName) || s.Name.Trim().ToLower().Contains(specification.SearchName))

                  )
        {

        }
    }
}
