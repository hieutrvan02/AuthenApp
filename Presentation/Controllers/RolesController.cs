using AuthenApp.Domain.Enitities;
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
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET api/roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        // POST api/roles
        [HttpPost]
        public async Task<ActionResult> AddRole([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name cannot be null or empty.");
            }

            var role = new IdentityRole(roleName.Trim());
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetRoles), new { id = role.Id }, role);
            }

            return BadRequest(result.Errors);
        }
    }
}
