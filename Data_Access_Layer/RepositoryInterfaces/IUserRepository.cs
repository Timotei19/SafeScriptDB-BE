using Data_Access_Layer.Repositories;
using Models.DTOs;
using Models.Entities;

namespace Data_Access_Layer.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();

        Task<PagedResult<UserDTO>> GetPagedUsersWithRolesAsync(PagedUserRequest pagedUserRequest);

        Task<User> GetUserById(int userId);

        Task DeleteUser(User user);

        Task UpdateUserAsync(User user);

        Task<User> GetUserwithRoleById(int userId);
    }
}
