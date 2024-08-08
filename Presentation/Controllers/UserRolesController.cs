using AuthenApp.Domain.Enitities;
using AuthenApp.Infrastructure.Services;
using AuthenApp.Infrastructure.Services.Impl;
using AuthenApp.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenApp.Presentation.Controllers
{
    [Authorize(Roles = nameof(UserRoles.Admin))]
    [ApiController]
    [Route("api/[controller]")]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRolesController"/> class.
        /// </summary>
        /// <param name="userRoleService">The user role service.</param>
        public UserRolesController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        /// <summary>
        /// Retrieves roles assigned to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A <see cref="ManageUserRolesViewModel"/> containing the user's roles.</returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<ManageUserRolesViewModel>> GetUserRoles(string userId)
        {
            var model = await _userRoleService.GetUserRolesAsync(userId);
            if (model == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }
            return Ok(model);
        }

        /// <summary>
        /// Updates roles for a specific user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <param name="model">The model containing updated role information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateUserRoles(string id, [FromBody] ManageUserRolesViewModel model)
        {
            var result = await _userRoleService.UpdateUserRolesAsync(id, model);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent(); // 204 No Content
        }
    }
}
