using AuthenApp.Domain.Enitities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenApp.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public ProductController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpGet("permissions")]
        public async Task<IActionResult> GetPermissions()
        {
            var permissions = new
            {
                Create = await _authorizationService.AuthorizeAsync(User, Permissions.Products.Create),
                View = await _authorizationService.AuthorizeAsync(User, Permissions.Products.View),
                Edit = await _authorizationService.AuthorizeAsync(User, Permissions.Products.Edit),
                Delete = await _authorizationService.AuthorizeAsync(User, Permissions.Products.Delete)
            };

            return Ok(permissions);
        }
    }
}
