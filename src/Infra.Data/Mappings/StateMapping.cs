using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Mappings
{
    public class StateMapping : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.ToTable("States", "dbo");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Name");

            builder.HasOne(s => s.Governor)
                .WithOne(s => s.State)
                .HasForeignKey<Governor>(s => s.StateId)
                .HasConstraintName("FK_Governor_State");
        }
    }
}
