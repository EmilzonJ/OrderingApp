using System;
using System.Threading.Tasks;
using Domain.Repositories;

namespace Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        Task<int> SaveAsync();
    }
}