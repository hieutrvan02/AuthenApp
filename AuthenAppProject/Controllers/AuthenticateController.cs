using AuthenApp.Application.Enitities;
using AuthenApp.Application.Services;
using AuthenApp.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenApp.Application.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRoleService _roleService;
        private readonly ITokenService _tokenService;

        public AuthenticateController(
            UserManager<IdentityUser> userManager,
            IRoleService roleService,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _roleService = roleService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="model">The login model containing username and password.</param>
        /// <returns>A JWT token and its expiration time.</returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = _tokenService.GetClaims(user, userRoles);
            var token = _tokenService.GenerateToken(authClaims);

            return Ok(new
            {
                token,
                expiration = DateTime.Now.AddHours(3) // or use token.ValidTo if available
            });
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="model">The registration model containing user details.</param>
        /// <returns>A response indicating the success or failure of the registration.</returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return Conflict(new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        /// <summary>
        /// Registers a new admin user.
        /// </summary>
        /// <param name="model">The registration model containing user details.</param>
        /// <returns>A response indicating the success or failure of the registration.</returns>
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return Conflict(new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            await _roleService.AssignRolesAsync(user, new[] { UserRoles.Admin, UserRoles.User });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
    }
}
