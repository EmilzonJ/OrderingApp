using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetById(Guid id);
        
        Task AddAsync(TEntity entity);
        Task Update(TEntity entity);
        
        Task Remove(TEntity entity);
    }
}