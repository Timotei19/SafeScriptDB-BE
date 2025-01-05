using Business_Logic_Layer.Interfaces;
using Data_Access_Layer.Repositories;
using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Models.DTOs;
using Models.Entities;
using Models.Models;

namespace Business_Logic_Layer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuditRepository _auditRepository;
        public UserService(IUserRepository userRepository, IAuditRepository auditRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _auditRepository = auditRepository;
            _userManager = userManager;
        }

        public async Task<List<Models.Entities.User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<List<UserDTO>> GetAllUsersWithRolesAsync()
        {
            return await _userRepository.GetAllUsersWithRolesAsync();
        }

        public async Task<IEnumerable<Role>> GetAllUserRolesAsync(int userId)
        {
            return await _userRepository.GetAllUserRolesAsync(userId);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                throw new Exception($"User not found.");
            }
            try
            {

                await _userRepository.DeleteUser(user);
            }
            catch (Exception ex)
            {
                throw new Exception("User doesn't exists");
            }
        }

        public async Task<UserStatisticsReponse> GetUserStatisticsAsync(UserStatisticsRequest request)
        {
            var user = await _userRepository.GetUserById(request.UserId);

            if (user == null)
            {
                throw new Exception("User doesn't exists");
            }

            var userStatistics = await _auditRepository.GetUserStatisticsAsync(request);

            userStatistics.Email = user.Email;

            return userStatistics;
        }
    }
}
