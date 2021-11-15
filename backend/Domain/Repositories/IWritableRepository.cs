using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IWritableRepository<T, in TPKey> : IReadOnlyRepository<T, TPKey>
    {
        Task<T> AddAsync(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(TPKey id, bool softDelete);
    }
}