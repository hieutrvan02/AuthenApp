using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthenApp.Application.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityUser> GetCurrentUserAsync(ClaimsPrincipal userPrincipal)
        {
            return await _userManager.GetUserAsync(userPrincipal);
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsersExceptCurrentAsync(ClaimsPrincipal userPrincipal)
        {
            var currentUser = await _userManager.GetUserAsync(userPrincipal);
            if (currentUser == null)
            {
                return Enumerable.Empty<IdentityUser>();
            }

            return await _userManager.Users
                .Where(user => user.Id != currentUser.Id)
                .ToListAsync();
        }
    }
}
