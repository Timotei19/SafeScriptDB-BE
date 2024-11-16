using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly string _connectionString;
        private readonly ApplicationDbContext _context;

        public UserRepository(IConfiguration configuration, ApplicationDbContext context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<UserDTO>> GetAllUsersWithRolesAsync()
        {
            var users = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role) // Include the Role entity through UserRoles
                .Select(u => new UserDTO
                {
                    ID = u.ID,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Roles = u.UserRoles.Select(ur => ur.Role.RoleName).ToList()
                })
                .ToListAsync();

            return users;
        }

        public async Task<List<Role>> GetAllUserRolesAsync(int userId)
        {
            var user = await _context.Users.Where(u => u.ID == userId)
                            .Include(u => u.UserRoles)
                                .ThenInclude(ur => ur.Role)
                                    .FirstOrDefaultAsync();
        }
    }
}
