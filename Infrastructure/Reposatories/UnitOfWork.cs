using Core.Context;
using Core.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Reposatories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _Context;
        private Hashtable _repositories;

        public UnitOfWork(StoreDbContext context)
        {
            _Context = context;
        }
        public async Task<int> Complete()
            => await _Context.SaveChangesAsync();

        public void Dispose()
            => _Context.Dispose();

        public IGenericReposatory<TEntity> Reposatory<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericReposatory<>);

                var repsitoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _Context);
                _repositories[type] = repsitoryInstance;
            }

            return (IGenericReposatory<TEntity>)_repositories[type];


            
            
        }
    }
}
