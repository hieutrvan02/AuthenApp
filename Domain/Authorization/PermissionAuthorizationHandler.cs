using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthenApp.Domain.Authorization
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PermissionAuthorizationHandler(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                return;
            }

            var userClaims = context.User.Claims.ToList();

            // Retrieve roles directly from the user's claims
            var userRoles = userClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            // Aggregate all permissions from the roles
            var allPermissions = new List<Claim>();
            foreach (var role in userRoles)
            {
                var roleObj = await _roleManager.FindByNameAsync(role);
                if (roleObj != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(roleObj);
                    allPermissions.AddRange(roleClaims);
                }
            }

            // Check if any of the permissions match the requirement
            var hasPermission = allPermissions.Any(x => x.Type == "Permission" &&
                                                        x.Value == requirement.Permission &&
                                                        x.Issuer == "LOCAL AUTHORITY");

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}
