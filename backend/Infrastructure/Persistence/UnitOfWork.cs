using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Repositories;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDataContext _context;

        public UnitOfWork(AppDataContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
            Categories = new CategoryRepository(_context);
        }
        
        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        
        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }

    }
}