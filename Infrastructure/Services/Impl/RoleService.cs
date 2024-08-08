using AuthenApp.Domain.Enitities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityRole> FindRoleByNameAsync(string name)
        {
            return await _roleManager.FindByNameAsync(name);
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role name cannot be null or empty." });
            }

            var role = new IdentityRole(roleName.Trim());
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            // Tìm vai trò theo roleId
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                // Vai trò không tồn tại
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
            }

            try
            {
                // Xóa vai trò
                return await _roleManager.DeleteAsync(role);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return IdentityResult.Failed(new IdentityError { Description = $"An error occurred while deleting the role: {ex.Message}" });
            }
        }
    }
}
