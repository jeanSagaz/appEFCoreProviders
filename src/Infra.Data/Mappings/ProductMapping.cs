using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Name");

            builder.Property(c => c.Active)
                .IsRequired()
                .HasColumnName("Active");

            builder.Property(c => c.DateCreated)
                .IsRequired()
                .HasColumnName("DateCreated");

            builder.Property(c => c.DateUpdated)                
                .HasColumnName("DateUpdated");

            // Many to many
            builder
                .HasMany(c => c.Customers)
                .WithMany(c => c.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomersProducts",
                    p => p.HasOne<Customer>().WithMany().HasForeignKey("CustomerId"),
                    p => p.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                    p =>
                    {
                        //p.Property<DateTime>("DateCreated").HasDefaultValue(DateTime.UtcNow);
                    }
                );
                // desabilita o delete cascade
                //.OnDelete(DeleteBehavior.ClientCascade);
                //.OnDelete(DeleteBehavior.Restrict);
        }
    }
}
