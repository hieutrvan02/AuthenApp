using AuthenApp.Application.Enitities;
using AuthenApp.Application.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

namespace AuthenApp.Application.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new IdentityUser
            {
                UserName = "basicuser@gmail.com",
                Email = "basicuser@gmail.com",
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, UserRoles.User.ToString());
                }
            }
        }
        public static async Task SeedSuperAdminAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new IdentityUser
            {
                UserName = "superadmin@gmail.com",
                Email = "superadmin@gmail.com",
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, UserRoles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, UserRoles.User.ToString());
                }
                await roleManager.SeedClaimsForAdmin();
            }
        }
        private async static Task SeedClaimsForAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("Admin");
            await roleManager.AddPermissionClaim(adminRole, "Products");
        }
        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            // Retrieve all existing claims of the role
            var allClaims = await roleManager.GetClaimsAsync(role);

            // Generate permissions for the specific module
            var allPermissions = Permissions.GeneratePermissionsForModule(module);

            // Iterate through each permission and add it to the role if it doesn't already exist
            foreach (var permission in allPermissions)
            {
                await ClaimsHelper.AddPermissionClaim(roleManager, role, permission);
            }
        }
    }
}
