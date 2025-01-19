using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Smo;
using Models.AppConstants;
using Models.Entities;
using System.CodeDom;
using User = Models.Entities.User;

namespace Data_Access_Layer.Repositories
{
    public class UserRepository : IUserRepository
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
                .Include(u => u.UserRole)
                   .ThenInclude(ur => ur.Role)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserName = u.UserName,
                    Role =  u.UserRole.Role.RoleName,
                }).ToListAsync();

            return users;
        }

       /* public async Task<IEnumerable<Role>> GetAllUserRolesAsync(int userId)
        {
            var roles = await _context.Users
                    .Where(u => u.Id == userId)
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .SelectMany(u => u.UserRoles.Select(ur => ur.Role))
                    .ToListAsync();

            return roles;
        }*/

        public async Task<User> GetUserById(int userId)
        {
            var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> GetUserwithRoleById(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRole)
                   .Where(u => u.Id == userId).FirstOrDefaultAsync();

            return user;
        }

        public async Task DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "The user to delete cannot be null.");
            }
            var userRoles = _context.UserRole.Where(ur => ur.UserId == user.Id);
            _context.UserRole.RemoveRange(userRoles);

            if (_context.Entry(user).State == EntityState.Detached)
            {
                _context.Users.Attach(user);
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }
    }
}
