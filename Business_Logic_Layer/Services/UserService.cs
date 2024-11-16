using Business_Logic_Layer.Interfaces;
using Data_Access_Layer.Repositories;
using Data_Access_Layer.RepositoryInterfaces;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<List<UserDTO>> GetAllUsersWithRolesAsync()
        {
            return await _userRepository.GetAllUsersWithRolesAsync();
        }
    }
}
