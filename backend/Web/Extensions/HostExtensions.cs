using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace Web.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();
                var dbContextName = typeof(TContext).Name;

                try
                {
                    logger.LogInformation(
                        $"Migrando la base de datos asociada con el contexto {typeof(TContext).Name}");

                    var retry = Policy.Handle<SqlException>()
                        .WaitAndRetry(
                            retryCount: 3,
                            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(5),
                            onRetry: (exception, retryCount, context) =>
                            {
                                logger.LogError(
                                    $"Intentando de nuevo {retryCount} de {context.PolicyKey} en {context.OperationKey}, debido a {exception}");
                            }
                        );

                    retry.Execute(() =>
                    {
                        context?.Database.Migrate();
                    });
                    
                    logger.LogInformation($"Base de datos migrada asociada con el contexto {dbContextName}");
                }
                catch (SqlException exception)
                {
                    logger.LogError(exception, $"Se produjo un error al migrar la base de datos utilizada en el contexto {dbContextName}");
                }
            }

            return host;
        }
    }
}