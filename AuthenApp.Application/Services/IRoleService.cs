using AuthenApp.Application.Enitities;
using Microsoft.AspNetCore.Identity;

namespace AuthenApp.Application.Services
{
    public interface IRoleService
    {
        Task AssignRolesAsync(IdentityUser user, IEnumerable<UserRoles> roles);
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityRole> FindRoleByNameAsync(string name);
        Task<IdentityResult> DeleteRoleAsync(string roleId);
    }
}
