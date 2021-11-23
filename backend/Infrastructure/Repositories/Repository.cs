using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDataContext Context;

        public Repository(AppDataContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            Task.CompletedTask.Wait();
        }

        public async Task Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            Task.CompletedTask.Wait();
        }

        public async Task Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            Task.CompletedTask.Wait();
        }
    }
}