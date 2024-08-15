using AuthenApp.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenApp.Application.Services.Impl
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleService"/> class.
        /// </summary>
        /// <param name="userManager">The user manager service.</param>
        /// <param name="signInManager">The sign-in manager service.</param>
        /// <param name="roleManager">The role manager service.</param>
        public UserRoleService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Retrieves roles assigned to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A <see cref="ManageUserRolesViewModel"/> containing the user's roles.</returns>
        public async Task<ManageUserRolesViewModel> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var viewModel = await BuildUserRolesViewModel(user);
            return new ManageUserRolesViewModel
            {
                UserId = userId,
                UserRoles = viewModel
            };
        }

        /// <summary>
        /// Updates roles for a specific user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <param name="model">The model containing updated role information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<IdentityResult> UpdateUserRolesAsync(string id, ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User with ID {id} not found." });
            }

            var result = await UpdateRolesForUserAsync(user, model.UserRoles);
            await _signInManager.RefreshSignInAsync(user);
            await Application.Seeds.DefaultUsers.SeedSuperAdminAsync(_userManager, _roleManager);

            return result;
        }

        private async Task<List<UserRolesViewModel>> BuildUserRolesViewModel(IdentityUser user)
        {
            var roles = _roleManager.Roles.ToList();
            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (var role in roles)
            {
                var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
                userRolesViewModel.Add(new UserRolesViewModel
                {
                    RoleName = role.Name,
                    Selected = isInRole
                });
            }

            return userRolesViewModel;
        }

        private async Task<IdentityResult> UpdateRolesForUserAsync(IdentityUser user, IEnumerable<UserRolesViewModel> roles)
        {
            var existingRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, existingRoles);

            if (!removeResult.Succeeded)
            {
                return removeResult;
            }

            var selectedRoles = roles.Where(r => r.Selected).Select(r => r.RoleName);
            return await _userManager.AddToRolesAsync(user, selectedRoles);
        }
    }
}
