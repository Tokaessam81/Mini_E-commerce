using E_commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Data.Configration
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Category)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.ProductCode)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.HasIndex(p => p.ProductCode)
                   .IsUnique();

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.ImagePath)
                   .IsRequired();

            builder.Property(p => p.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.MinimumQuantity)
                   .IsRequired();

            builder.Property(p => p.DiscountRate)
                   .IsRequired(false);
        }
    }
}
