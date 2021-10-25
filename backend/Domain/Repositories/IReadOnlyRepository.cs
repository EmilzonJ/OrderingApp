using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IReadOnlyRepository <T, in TPKey> 
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id);
        IQueryable<T> Query { get; }
    }
}