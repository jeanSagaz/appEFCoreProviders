using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Mappings
{
    public class GovernorMapping : IEntityTypeConfiguration<Governor>
    {
        public void Configure(EntityTypeBuilder<Governor> builder)
        {
            builder.ToTable("Governors", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Name");

            builder.Property(c => c.StateId)
                .IsRequired();
        }
    }
}
