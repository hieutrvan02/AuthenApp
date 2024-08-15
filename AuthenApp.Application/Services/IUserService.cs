using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthenApp.Application.Services
{
    public interface IUserService
    {
        Task<IdentityUser> GetCurrentUserAsync(ClaimsPrincipal userPrincipal);
        Task<IEnumerable<IdentityUser>> GetAllUsersExceptCurrentAsync(ClaimsPrincipal userPrincipal);
    }
}
