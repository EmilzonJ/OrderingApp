using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class WritableRepository<T, TPKey> : IWritableRepository<T, TPKey> where T : class, IAuditEntity
    {

        private readonly AppDataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WritableRepository(AppDataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>()
                .Where(_ => !_.IsDeleted).ToListAsync();
        }

        public Task<T> GetById(Guid id)
        {
            throw new NotImplementedException();
        }
        

        public IQueryable<T> Query { get; }
        
        
        public async Task<T> Create(T entity)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var date = DateTime.Now;

            entity.CreatedBy = userId;
            entity.UpdatedBy = userId;
            entity.CreatedDate = date;
            entity.IsDeleted = false;

            _context.Set<T>().Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> Update(T entity)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var date = DateTime.UtcNow;
            entity.UpdatedBy = userId;
            entity.UpdatedDate = date;
            
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> Delete(TPKey id, bool softDelete)
        {
            try
            {
                if (softDelete)
                {
                    var entity = await _context.Set<T>().FindAsync(id);
                    entity.IsDeleted = true;
                    _context.Set<T>().Update(entity);
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    var entity = await _context.Set<T>().FindAsync(id);
                    _context.Set<T>().Remove(entity);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}