using AuthenApp.Domain.Enitities;
using Microsoft.AspNetCore.Identity;

namespace AuthenApp.Infrastructure.Services
{
    public interface IRoleService
    {
        Task AssignRolesAsync(IdentityUser user, IEnumerable<UserRoles> roles);
    }
}
