using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Mappings
{
    public class CustomerMapping : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Name");

            builder.Property(c => c.BirthDate)
                .IsRequired()                
                .HasColumnName("BirthDate");

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Email");

            builder.Property(c => c.DateCreated)
                .IsRequired()
                .HasColumnName("DateCreated");

            builder.Property(c => c.DateUpdated)
                .HasColumnName("DateUpdated");
        }
    }
}
