using AuthenApp.Application.Services.Impl;
using AuthenApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenApp.Application.Mappers;

namespace AuthenApp.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddAutoMapper(typeof(AutoMapperProfile));
            return services;
        }
    }
}
