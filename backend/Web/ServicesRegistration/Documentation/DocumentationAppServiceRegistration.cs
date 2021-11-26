using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Web.ServicesRegistration.Documentation
{
    public static class DocumentationAppServiceRegistration
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services,
            IConfiguration configuraton)
        {
            services.AddSwaggerGen();
            
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
                            new Uri(configuraton["AUTH0_DOMAIN"] + "authorize?audience=" +
                                    configuraton["AUTH0_API_IDENTIFIER"])
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

            return services;
        }
    }
}