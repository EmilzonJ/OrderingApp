using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.ServicesRegistration.Authentication;
using Web.ServicesRegistration.Documentation;

namespace Web.ServicesRegistration
{
    public static class WebServiceRegistration
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration, string clientAppOrigins)
        {
            services.AddMvc();
            services.AddSignalR();
            services.AddControllers().AddJsonOptions(options => // SignalR
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddCors(options =>
            {
                options.AddPolicy(
                    clientAppOrigins,
                    builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithOrigins("http://localhost:3000/")
                            .AllowCredentials();
                    });
            });

            services.AddSwaggerService(configuration);
            services.AddAuthenticationService(configuration);

            return services;
        }
    }
}