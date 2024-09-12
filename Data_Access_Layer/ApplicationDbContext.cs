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

    public DbSet<Audit> Audits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuditConfiguration());
    }
}
