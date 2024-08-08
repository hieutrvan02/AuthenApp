using AuthenApp.Presentation.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenApp.Infrastructure.Services
{
    public interface IPermissionService
    {
        Task<PermissionViewModel> BuildPermissionViewModel(string roleId);
        Task UpdateRoleClaims(IdentityRole role, IList<RoleClaimsViewModel> roleClaims);
        Task AddPermissionsToRole(IdentityRole role, string module);
        List<RoleClaimsViewModel> GetAllPermissions(string roleId);
    }
}
