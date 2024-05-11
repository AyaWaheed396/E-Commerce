using Core.Context;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Reposatories
{
    public class GenericReposatory<T> : IGenericReposatory<T> where T : BaseEntity
    {
        private readonly StoreDbContext _Context;

        public GenericReposatory(StoreDbContext context)
        {
            _Context = context;
        }

        public async Task Add(T entity)
            => await _Context.Set<T>().AddAsync(entity); 

        public void Delete(T entity)
            => _Context.Set<T>().Remove(entity);

        public void Update(T entity)
            => _Context.Set<T>().Update(entity);

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
			if (typeof(T) == typeof(Product))
				return (IReadOnlyList<T>)await _Context.Set<Product>().Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();

			return await _Context.Set<T>().ToListAsync();
		}
            


        public async Task<T> GetByIdAsync(int? id)
            => await _Context.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> GetAllWithSpecificationsAsync(ISpecification<T> specification)
            => await ApplySpecification(specification).ToListAsync();


        public async Task<T> GetEntityWithSpecificationsAsync(ISpecification<T> specification)
            => await ApplySpecification(specification).FirstOrDefaultAsync();

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
            => SpecificationEvaluater<T>.GetQuery(_Context.Set<T>().AsQueryable(), specification);

        public async Task<int> CountAsync(ISpecification<T> specification)
            => await ApplySpecification(specification).CountAsync();
    }
}
