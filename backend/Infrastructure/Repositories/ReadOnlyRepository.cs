using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReadOnlyRepository<T, TPKey> : IReadOnlyRepository<T, TPKey> where T : class, IAuditEntity
    {
        private AppDataContext _context;

        public ReadOnlyRepository(AppDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .Where(_ => !_.IsDeleted).ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> Query { get; }
    }
}