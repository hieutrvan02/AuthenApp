using AuthenApp.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenApp.Application.Services
{
    public interface IUserRoleService
    {
        Task<ManageUserRolesViewModel> GetUserRolesAsync(string userId);
        Task<IdentityResult> UpdateUserRolesAsync(string id, ManageUserRolesViewModel model);
    }
}
