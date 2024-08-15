using AuthenApp.Application.Enitities;
using AuthenApp.Application.Helpers;
using AuthenApp.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenApp.Application.Services.Impl
{
    public class PermissionService : IPermissionService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public PermissionService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<PermissionViewModel> BuildPermissionViewModel(string roleId)
        {
            var model = new PermissionViewModel { RoleId = roleId };
            var allPermissions = GetAllPermissions(roleId);

            var role = await _roleManager.FindByIdAsync(roleId);
            var claims = await _roleManager.GetClaimsAsync(role);
            var roleClaimValues = claims.Select(c => c.Value).ToList();
            var authorizedClaims = allPermissions.Select(p => p.Value).Intersect(roleClaimValues).ToList();

            foreach (var permission in allPermissions)
            {
                permission.Selected = authorizedClaims.Contains(permission.Value);
            }
            model.RoleClaims = allPermissions;
            return model;
        }

        public List<RoleClaimsViewModel> GetAllPermissions(string roleId)
        {
            var allPermissions = new List<RoleClaimsViewModel>();
            allPermissions.GetPermissions(typeof(Permissions.Products), roleId);
            return allPermissions;
        }

        public async Task UpdateRoleClaims(IdentityRole role, IList<RoleClaimsViewModel> roleClaims)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in currentClaims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            var selectedClaims = roleClaims.Where(rc => rc.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role, claim.Value);
            }
        }

        public async Task AddPermissionsToRole(IdentityRole role, string module)
        {
            var permissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in permissions)
            {
                await ClaimsHelper.AddPermissionClaim(_roleManager, role, permission);
            }
        }
    }
}
