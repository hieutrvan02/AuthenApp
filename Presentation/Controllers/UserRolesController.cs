using AuthenApp.Domain.Enitities;
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
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET api/userroles/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<ManageUserRolesViewModel>> GetUserRoles(string userId)
        {
            var viewModel = new List<UserRolesViewModel>();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleName = role.Name,
                    Selected = await _userManager.IsInRoleAsync(user, role.Name)
                };
                viewModel.Add(userRolesViewModel);
            }

            var model = new ManageUserRolesViewModel
            {
                UserId = userId,
                UserRoles = viewModel
            };

            return Ok(model);
        }

        // POST api/userroles/{id}
        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateUserRoles(string id, [FromBody] ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            result = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            await _signInManager.RefreshSignInAsync(currentUser);
            await Infrastructure.Seeds.DefaultUsers.SeedSuperAdminAsync(_userManager, _roleManager);

            return NoContent(); // 204 No Content
        }
    }
}
