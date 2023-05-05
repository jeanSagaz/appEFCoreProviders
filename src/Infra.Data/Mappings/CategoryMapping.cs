using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Mappings
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Name");

            builder.Property(c => c.Active)
                .IsRequired()
                .HasColumnName("Active");

            builder.HasMany(p => p.Products)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId)
                .HasConstraintName("FK_Categories_Products")
                // desabilita o delete cascade
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
