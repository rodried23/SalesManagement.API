using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesManagement.Domain.Entities;

namespace SalesManagement.Infrastructure.Data.Mappings
{
    public class BranchMap : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branches");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Address)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.City)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.State)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.IsActive)
                .IsRequired();

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            // Ignore domain events
            builder.Ignore(b => b.DomainEvents);
        }
    }
}