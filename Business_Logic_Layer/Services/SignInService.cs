using Business_Logic_Layer.Interfaces;
using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Models.AppConstants;
using Models.Models;

namespace Business_Logic_Layer.Services
{
    public class SignInService : ISignInService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public SignInService(UserManager<ApplicationUser> userManager, IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _userManager = userManager;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<(bool Succeeded, string Message, IEnumerable<string> Errors)> RegisterUser(RegisterModel register)
        {
            var user = new ApplicationUser
            {
                UserName = register.Email,
                Email = register.Email,
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            var createResult = await _userManager.CreateAsync(user, register.Password);

            if (!createResult.Succeeded)
            {
                return (false, "User creation failed.", createResult.Errors.Select(e => e.Description));
            }

            var createdUser = await _userManager.FindByNameAsync(user.Email);

            var roleToUser = await _userRepository.GetUserById(createdUser.Id);

            await _roleRepository.AddRole(roleToUser, (int)Enums.Role.User);

            return (true, "User registered successfully.", null);
        }
    }
}
