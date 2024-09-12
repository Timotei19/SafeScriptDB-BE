using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.DbConfigurations
{
    public class AuditItemConfiguration : IEntityTypeConfiguration<AuditItem>
    {
        public void Configure(EntityTypeBuilder<AuditItem> builder)
        {
            builder.ToTable("AuditItem");

            builder.HasKey(ai => ai.Id);

            builder.HasOne(ai => ai.Audit)
            .WithMany(a => a.AuditItems)
            .HasForeignKey(ai => ai.AuditId);
        }
    }
}
