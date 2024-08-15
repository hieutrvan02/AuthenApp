using AuthenApp.Application.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Security.Claims;

namespace AuthenApp.Application.Helpers
{
    public static class ClaimsHelper
    {
        public static void GetPermissions(this List<RoleClaimsViewModel> allPermissions, Type policy, string roleId)
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo fi in fields)
            {
                allPermissions.Add(new RoleClaimsViewModel { Value = fi.GetValue(null).ToString(), Type = "Permissions" });
            }
        }
        
        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            // Check if the permission already exists in the role's claims
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                // Add the permission to the role if it doesn't exist
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}
