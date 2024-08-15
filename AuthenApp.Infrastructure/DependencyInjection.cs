using AuthenApp.Application.Repositories.Impl;
using AuthenApp.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;
using AuthenApp.Application.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace AuthenApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
        {
            services.AddScoped<ISuperHeroRepository, SuperHeroRepository>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            return services;
        }
    }
}
