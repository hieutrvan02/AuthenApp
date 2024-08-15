using AuthenApp.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenApp.Application.Services
{
    public interface IPermissionService
    {
        Task<PermissionViewModel> BuildPermissionViewModel(string roleId);
        Task UpdateRoleClaims(IdentityRole role, IList<RoleClaimsViewModel> roleClaims);
        Task AddPermissionsToRole(IdentityRole role, string module);
        List<RoleClaimsViewModel> GetAllPermissions(string roleId);
    }
}
