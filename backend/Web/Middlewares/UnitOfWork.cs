using System;
using System.Threading.Tasks;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;

namespace Web.Middlewares
{
    public class UnitOfWork
    {
        private readonly RequestDelegate _next;

        public UnitOfWork(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AppDataContext dbContext)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                await transaction.CommitAsync();

                await _next(context);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}