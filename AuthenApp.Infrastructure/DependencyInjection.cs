using Microsoft.Extensions.DependencyInjection;
using AuthenApp.Application.Authorization;
using Microsoft.AspNetCore.Authorization;
using AuthenApp.Infrastructure.Repositories.Impl;
using AuthenApp.Infrastructure.Repositories;

namespace AuthenApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            return services;
        }
    }
}
