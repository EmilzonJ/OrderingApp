using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query { get; }
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetById(Guid id);
        
        Task AddAsync(TEntity entity);
        Task Update(TEntity entity);
        
        Task Remove(TEntity entity);
    }
}