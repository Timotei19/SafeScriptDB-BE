using Data_Access_Layer.Repositories;
using Models.Entities;

namespace Business_Logic_Layer.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();

        Task<List<UserDTO>> GetAllUsersWithRolesAsync();
    }
}
