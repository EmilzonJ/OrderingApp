using System;
using System.Collections.Generic;
using Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Web.Middlewares;
using Web.Hubs;

namespace Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        // CORS
        private readonly string _clientAppOrigins = "ClientApp";

        //Swagger
        private const string ENV_AUTH0_DOMAIN = "AUTH0_DOMAIN";
        private const string ENV_AUTH0_API_IDENTIFIER = "AUTH0_API_IDENTIFIER";
        private const string ENV_AUTH0_CLIENT_ID = "AUTH0_CLIENT_ID";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #region Services

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            // SignalR
            services.AddSignalR();

            // MediatR
            // services.AddMediatR(typeof(IIdentityGenerator<>));

            // Auth0
            

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy(
                    _clientAppOrigins,
                    builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithOrigins("http://localhost:3000/")
                            .AllowCredentials();
                    });
            });

            // Swagger
            services.AddSwaggerGen(); //Register the Swagger generator, defining 1 or more Swagger documents
            ConfigureSwagger(services, Configuration);
            
            services.AddHttpContextAccessor();

            services.AddInfrastructureServices(Configuration); // Infrastructure layer Services
            services.AddApplicationService();                   // Application layer Services
            
            services.AddControllers().AddJsonOptions(options => // SignalR
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        }
        
        private void ConfigureSwagger(IServiceCollection services, IConfiguration configuration)
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        Scopes = new Dictionary<string, string>
                        {
                            {"openId", "Open Id"},
                            {"profile", "Profile"},
                            {"email", "Email"},
                        },
                        AuthorizationUrl =
                            new Uri(configuration[ENV_AUTH0_DOMAIN] + "authorize?audience=" +
                                    configuration[ENV_AUTH0_API_IDENTIFIER])
                    }
                }
            };

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Real Time App",
                    Version = "v1"
                });

                var security = new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new[]
                        {
                            "readAccess", "writeAccess"
                        }
                    }
                };
                swagger.AddSecurityDefinition("Bearer", securityScheme);
                swagger.AddSecurityRequirement(security);
            });
        }
        #endregion Services

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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