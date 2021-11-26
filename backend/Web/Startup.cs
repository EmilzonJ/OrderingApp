using System;
using System.Collections.Generic;
using Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Web.Middlewares;
using Web.Hubs;
using Web.ServicesRegistration;

namespace Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        private readonly string _clientAppOrigins = "ClientApp";

        //Swagger
        private const string ENV_AUTH0_CLIENT_ID = "AUTH0_CLIENT_ID";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddWebServices(Configuration, _clientAppOrigins);  // WebApi layer Services
            services.AddInfrastructureServices(Configuration);          // Infrastructure layer Services
            services.AddApplicationService();                           // Application layer Services
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Real-Time-App API v1");
                s.OAuthClientId(Configuration[ENV_AUTH0_CLIENT_ID]);
            });

            // Middlewares
            app.UseMiddleware<UnitOfWork>();
            app.UseMiddleware<ErrorHandlerMiddlerware>();
            
            // CORS
            app.UseCors(_clientAppOrigins);
            
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ProductEventsClientHub>("/products-events");
            });
        }
    }
}