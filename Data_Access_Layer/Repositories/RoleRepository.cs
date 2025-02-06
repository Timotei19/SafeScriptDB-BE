using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.Extensions.Configuration;
using Models.Entities;

namespace Data_Access_Layer.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly string _connectionString;
    private readonly ApplicationDbContext _context;

    public RoleRepository(IConfiguration configuration, ApplicationDbContext context)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _context = context;
    }

    public async Task AddRole(User user, int roleId)
    {
        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = roleId
        };

        await _context.UserRole.AddAsync(userRole);


        await _context.SaveChangesAsync();
    }
}

