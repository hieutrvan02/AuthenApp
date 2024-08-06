using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenApp.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("current")]
        public async Task<ActionResult<IdentityUser>> GetCurrentUser()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                return NotFound();
            }
            return Ok(currentUser);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetAllUsersExceptCurrent()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var allUsersExceptCurrentUser = await _userManager.Users
                .Where(a => a.Id != currentUser.Id)
                .ToListAsync();

            return Ok(allUsersExceptCurrentUser);
        }
    }
}
