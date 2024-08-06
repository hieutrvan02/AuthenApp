﻿using AuthenApp.Domain.Enitities;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace AuthenApp.Infrastructure.Seeds
{
    public class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (UserRoles role in Enum.GetValues(typeof(UserRoles)))
            {
                string roleName = role.ToString();
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
