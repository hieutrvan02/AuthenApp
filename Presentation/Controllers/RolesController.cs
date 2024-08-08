using AuthenApp.Domain.Enitities;
using AuthenApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenApp.Presentation.Controllers
{
    [Authorize(Roles = nameof(UserRoles.Admin))]
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>A list of roles.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        /// <summary>
        /// Adds a new role.
        /// </summary>
        /// <param name="roleName">The name of the role to add.</param>
        /// <returns>Result of the role creation.</returns>
        [HttpPost]
        public async Task<ActionResult> AddRole([FromBody] string roleName)
        {
            var result = await _roleService.CreateRoleAsync(roleName);

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetRoles), new { name = roleName }, await _roleService.FindRoleByNameAsync(roleName));
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Deletes a role by its ID.
        /// </summary>
        /// <param name="roleId">The ID of the role to delete.</param>
        /// <returns>Result of the role deletion.</returns>
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRoleAsync(string roleId)
        {
            var result = await _roleService.DeleteRoleAsync(roleId);

            if (result.Succeeded)
            {
                return NoContent(); // HTTP 204 No Content indicates successful deletion
            }

            return NotFound(); // HTTP 404 Not Found indicates that the role was not found
        }
    }
}
