using Data_Access_Layer.Repositories;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();

        Task<List<UserDTO>> GetAllUsersWithRolesAsync();
    }
}
