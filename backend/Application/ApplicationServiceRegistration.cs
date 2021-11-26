using Application.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            var localAssembly = typeof(ApplicationServiceRegistration).Assembly;
            services.AddMediatR(assemblies: localAssembly );
            services.AddAutoMapper(assemblies: localAssembly);
            services.AddValidatorsFromAssembly(assembly: localAssembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            
            return services;
        } 
    }
}