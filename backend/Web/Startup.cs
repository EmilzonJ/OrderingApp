using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Data;
using Data.Repositories;
using Domain.Repositories;
using Web.Services;
using Web.Services.Interfaces;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Web.CommandsValidator.ProductValidation;
using Web.Middlewares;
using Web.Hubs;

namespace Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        private readonly string _connectionString;

        // CORS
        private readonly string _clientAppOrigins = "ClientApp";

        //Swagger
        private const string ENV_AUTH0_DOMAIN = "AUTH0_DOMAIN";
        private const string ENV_AUTH0_API_IDENTIFIER = "AUTH0_API_IDENTIFIER";
        private const string ENV_AUTH0_CLIENT_ID = "AUTH0_CLIENT_ID";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _connectionString = Configuration["DB_CONNECTION"];
        }

        #region Services

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            // SignalR
            services.AddSignalR();

            // MediatR
            services.AddMediatR(typeof(IIdentityGenerator<>));

            // Auth0
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var enableHttps = Convert.ToBoolean(Configuration["AUTH0_ENABLE_HTTPS"]);
                    options.Authority = Configuration[ENV_AUTH0_DOMAIN];
                    options.Audience = Configuration[ENV_AUTH0_API_IDENTIFIER];
                    options.RequireHttpsMetadata = enableHttps;
                    
                    // SignalR
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/productHub"))
                            {
                                context.Token = accessToken;
                            }
                            
                            return Task.CompletedTask;
                        }
                    };
                });

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy(
                    _clientAppOrigins,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithExposedHeaders("content-disposition");
                    });
            });

            // Swagger
            services.AddSwaggerGen(); //Register the Swagger generator, defining 1 or more Swagger documents
            ConfigureSwagger(services, Configuration);

            // Entity Framework
            ConfigureDbContext(services);

            // AutoMapper
            services.AddAutoMapper(typeof(Startup));

            // Configure Data Dependencies
            ConfigureDataDependencies(services);
            
            services.AddControllers().AddFluentValidation(assembly =>
            {
                assembly.RegisterValidatorsFromAssemblyContaining<AddProductValidation>();
            });

            services.AddControllers().AddJsonOptions(options => // SignalR
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        }

        private void ConfigureDbContext(IServiceCollection services)
        {
            services.AddDbContext<AppDataContext>(options =>
                options.UseLazyLoadingProxies()
                    .UseSqlServer(_connectionString)
            );
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

        private void ConfigureDataDependencies(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            // Interfaces of repositories
            services.AddScoped(typeof(IReadOnlyRepository<,>), typeof(ReadOnlyRepository<,>));
            services.AddScoped(typeof(IWritableRepository<,>), typeof(WritableRepository<,>));

            services.AddSingleton<IIdentityGenerator<Guid>, GuidIdentityGenerator>();
        }

        #endregion Services

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDataContext dbContext)
        {
            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Real-Time-App API v1");
                s.OAuthClientId(Configuration[ENV_AUTH0_CLIENT_ID]);
            });

            // Entity Framework
            dbContext.Database.Migrate();

            // Middlewares
            app.UseMiddleware<UnitOfWork>();
            app.UseMiddleware<ErrorHandlerMiddlerware>();
            
            app.UseHttpsRedirection(); 
            // CORS
            app.UseCors(_clientAppOrigins);
            
            app.UseRouting();

            //Authentication
            app.UseAuthentication();

            // Authorization
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ProductHub>("/productHub");
            });
        }
    }
}