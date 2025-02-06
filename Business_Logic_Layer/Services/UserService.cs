using Business_Logic_Layer.Interfaces;
using Data_Access_Layer.Repositories;
using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Models.AppConstants;
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


        public async Task<PagedResult<UserDTO>> GetPagedUsersWithRolesAsync(PagedUserRequest pagedUserRequest)
        {
            return await _userRepository.GetPagedUsersWithRolesAsync(pagedUserRequest);
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
                throw new Exception($"User not found.");
            }

            var userStatistics = await _auditRepository.GetUserStatisticsAsync(request);

            userStatistics.Email = user.Email;

            return userStatistics;
        }

        public async Task<UserDTO> UpdateUserAsync(UserDTO updateUserDto)
        {
            var user = await _userRepository.GetUserwithRoleById(updateUserDto.Id);

            if (user == null)
            {
                return null;
            }

            user.UserName = updateUserDto.UserName;
            user.Email = updateUserDto.Email;

            if (Enum.TryParse(typeof(Enums.Role), updateUserDto.Role, true, out var roleEnum))
            {
                user.UserRole ??= new UserRole();

                user.UserRole.RoleId = (int)roleEnum;
            }
            else
            {
                var errorMessage = $"Invalid role: {updateUserDto.Role}";

                throw new ArgumentException(errorMessage);
            }


            await _userRepository.UpdateUserAsync(user);

            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = Enum.GetName(typeof(Enums.Role), user.UserRole.RoleId) ?? "Unknown"
            };
        }
    }
}
