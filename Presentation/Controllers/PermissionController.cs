using AuthenApp.Domain.Enitities;
using AuthenApp.Infrastructure.Helpers;
using AuthenApp.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenApp.Presentation.Controllers
{
    [Authorize(Roles = nameof(UserRoles.Admin))]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public PermissionController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET api/permission/{roleId}
        [HttpGet("{roleId}")]
        public async Task<ActionResult<PermissionViewModel>> GetPermissions(string roleId)
        {
            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();
            allPermissions.GetPermissions(typeof(Permissions.Products), roleId);

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound($"Role with ID {roleId} not found.");
            }

            model.RoleId = roleId;
            var claims = await _roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();

            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;

            return Ok(model);
        }

        // POST api/permission/update
        [HttpPost("update")]
        public async Task<IActionResult> UpdatePermissions([FromBody] PermissionViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.RoleId))
            {
                return BadRequest("Invalid model or RoleId.");
            }

            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound($"Role with ID {model.RoleId} not found.");
            }

            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role, claim.Value);
            }

            return NoContent(); // 204 No Content
        }
    }
}
