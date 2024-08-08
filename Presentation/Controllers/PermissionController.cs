using AuthenApp.Domain.Enitities;
using AuthenApp.Infrastructure.Helpers;
using AuthenApp.Infrastructure.Services;
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
        private readonly IPermissionService _permissionService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private const string RoleNotFoundMessage = "Role with ID {0} not found.";
        private const string InvalidModelMessage = "Invalid model or RoleId.";

        public PermissionController(IPermissionService permissionService, RoleManager<IdentityRole> roleManager)
        {
            _permissionService = permissionService;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Gets the permissions for a specific role.
        /// </summary>
        /// <param name="roleId">The ID of the role.</param>
        /// <returns>A view model containing the permissions for the role.</returns>
        [HttpGet("{roleId}")]
        public async Task<ActionResult<PermissionViewModel>> GetPermissions(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(string.Format(RoleNotFoundMessage, roleId));
            }

            var model = await _permissionService.BuildPermissionViewModel(roleId);
            return Ok(model);
        }

        /// <summary>
        /// Updates the permissions for a specific role.
        /// </summary>
        /// <param name="model">The view model containing the updated permissions.</param>
        /// <returns>No content if successful.</returns>
        [HttpPost("update")]
        public async Task<IActionResult> UpdatePermissions([FromBody] PermissionViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.RoleId))
            {
                return BadRequest(InvalidModelMessage);
            }

            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound(string.Format(RoleNotFoundMessage, model.RoleId));
            }

            await _permissionService.UpdateRoleClaims(role, model.RoleClaims);
            return NoContent();
        }

        /// <summary>
        /// Creates permissions for a specific module and assigns them to a role.
        /// </summary>
        /// <param name="model">The model containing the role ID and module name.</param>
        /// <returns>A message indicating the permissions were added.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreatePermissions([FromBody] CreatePermissionModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.RoleId) || string.IsNullOrEmpty(model.Module))
            {
                return BadRequest("Invalid model, RoleId, or Module.");
            }

            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound(string.Format(RoleNotFoundMessage, model.RoleId));
            }

            await _permissionService.AddPermissionsToRole(role, model.Module);
            return Ok($"Permissions for module '{model.Module}' have been added to role '{role.Name}'.");
        }
    }
}

