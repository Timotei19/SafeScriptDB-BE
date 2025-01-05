using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using SafeScriptDb_BE.AppConstants;

namespace Data_Access_Layer.Contexts
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DatabaseSettings.GetApplicationConnectionString());
            }
        }
    }
}
