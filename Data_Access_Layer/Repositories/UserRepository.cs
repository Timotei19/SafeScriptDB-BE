using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.DTOs;
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

        public async Task<PagedResult<UserDTO>> GetPagedUsersWithRolesAsync(PagedUserRequest pagedUserRequest)
        {
            var query = _context.Users
                .Include(u => u.UserRole)
                .ThenInclude(ur => ur.Role)
                .AsQueryable();


            query = pagedUserRequest.SortDesc
                ? query.OrderByDescending(e => EF.Property<object>(e, pagedUserRequest.SortBy))
                : query.OrderBy(e => EF.Property<object>(e, pagedUserRequest.SortBy));

            var totalRecords = await query.CountAsync();
            var users = await query.Skip((pagedUserRequest.Page - 1) * pagedUserRequest.PageSize)
                                   .Take(pagedUserRequest.PageSize)
                                   .Select(u => new UserDTO
                                   {
                                       Id = u.Id,
                                       UserName = u.UserName,
                                       Email = u.Email,
                                       Role = u.UserRole.Role.RoleName
                                   })
                                   .ToListAsync();

            return new PagedResult<UserDTO>
            {
                Records = users,
                TotalRecords = totalRecords
            };
        }


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
