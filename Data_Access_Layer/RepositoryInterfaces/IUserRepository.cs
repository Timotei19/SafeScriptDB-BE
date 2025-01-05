using Data_Access_Layer.Repositories;
using Models.Entities;

namespace Data_Access_Layer.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();

        Task<List<UserDTO>> GetAllUsersWithRolesAsync();

        Task<IEnumerable<Role>> GetAllUserRolesAsync(int userId);

        Task<User> GetUserById(int userId);

        Task DeleteUser(User user);
    }
}
