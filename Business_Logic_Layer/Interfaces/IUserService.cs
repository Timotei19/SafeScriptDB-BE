using Data_Access_Layer.Repositories;
using Models.DTOs;
using Models.Entities;

namespace Business_Logic_Layer.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();

        Task<List<UserDTO>> GetAllUsersWithRolesAsync();

        //Task<IEnumerable<Role>> GetAllUserRolesAsync(int userId);

        Task DeleteUserAsync(int userId);

        Task<UserStatisticsReponse> GetUserStatisticsAsync(UserStatisticsRequest request);
        Task<UserDTO> UpdateUserAsync(UserDTO updateUserDto);
    }
}
