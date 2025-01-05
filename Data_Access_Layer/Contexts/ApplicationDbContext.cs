using Data_Access_Layer.DbConfigurations;
using Microsoft.EntityFrameworkCore;
using Models.DbConfigurations;
using Models.Entities;

namespace Data_Access_Layer;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<Models.Entities.Audit> Audits { get; set; }
    public DbSet<AuditItem> AuditItems { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Models.Entities.User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRole { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuditConfiguration());
        modelBuilder.ApplyConfiguration(new AuditItemConfiguration());
        modelBuilder.ApplyConfiguration(new StatusConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
    }
}
