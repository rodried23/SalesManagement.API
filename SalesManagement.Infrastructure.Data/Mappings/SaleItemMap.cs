using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesManagement.Domain.Entities;

namespace SalesManagement.Infrastructure.Data.Mappings
{
    public class SaleItemMap : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.SaleId)
                .IsRequired();

            builder.Property(i => i.ProductId)
                .IsRequired();

            builder.Property(i => i.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.Quantity)
                .IsRequired();

            builder.Property(i => i.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.Discount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.CreatedAt)
                .IsRequired();

            // Computed columns
            builder.Property(i => i.TotalPrice)
                .HasComputedColumnSql("(UnitPrice * Quantity) - Discount", stored: true)
                .HasColumnType("decimal(18,2)");

            // Ignore domain events
            builder.Ignore(i => i.DomainEvents);
        }
    }
}