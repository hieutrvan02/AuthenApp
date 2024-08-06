using AuthenApp.Domain.Enitities;
using Microsoft.AspNetCore.Identity;

namespace AuthenApp.Infrastructure.Services.Impl
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task AssignRolesAsync(IdentityUser user, IEnumerable<UserRoles> roles)
        {
            foreach (var role in roles)
            {
                string roleName = role.ToString();
                if (await _roleManager.RoleExistsAsync(roleName))
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}
