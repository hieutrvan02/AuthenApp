using AuthenApp.Application.Enitities;
using AuthenApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenApp.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets the currently authenticated user.
        /// </summary>
        /// <returns>The current user.</returns>
        [Authorize(Roles = nameof(UserRoles.User))]
        [HttpGet("current")]
        public async Task<ActionResult<IdentityUser>> GetCurrentUser()
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound("Current user not found.");
            }
            return Ok(user);
        }

        /// <summary>
        /// Gets all users except the currently authenticated user.
        /// </summary>
        /// <returns>A list of all users except the current user.</returns>
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetAllUsersExceptCurrent()
        {
            var users = await _userService.GetAllUsersExceptCurrentAsync(HttpContext.User);
            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }
    }
}
