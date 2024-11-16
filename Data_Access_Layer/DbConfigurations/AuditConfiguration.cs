using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.DbConfigurations
{
    public class AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.ToTable("Audit");

            builder.HasKey(a => a.Id);

            builder.HasMany(a => a.AuditItems)
            .WithOne(ai => ai.Audit)
            .HasForeignKey(ai => ai.AuditId);

            builder.HasOne<Status>()
                   .WithMany()
                   .HasForeignKey(a => a.StatusId);
        }
    }
}
