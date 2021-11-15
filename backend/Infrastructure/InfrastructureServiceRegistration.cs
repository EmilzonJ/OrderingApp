using System;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Services.Interfaces;

namespace Infrastructure
{
    public static class InfraestructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(configuration["DB_CONNECTION"])));

            services.AddScoped(typeof(IReadOnlyRepository<,>), typeof(ReadOnlyRepository<,>));
            services.AddScoped(typeof(IWritableRepository<,>), typeof(WritableRepository<,>));
            services.AddSingleton<IIdentityGenerator<Guid>, GuidIdentityGenerator>();

            return services;
        }
    }
}