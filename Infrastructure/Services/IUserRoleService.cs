using AuthenApp.Presentation.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenApp.Infrastructure.Services
{
    public interface IUserRoleService
    {
        Task<ManageUserRolesViewModel> GetUserRolesAsync(string userId);
        Task<IdentityResult> UpdateUserRolesAsync(string id, ManageUserRolesViewModel model);
    }
}
