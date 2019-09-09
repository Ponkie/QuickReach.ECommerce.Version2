using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickReach.ECommerce.Domain.Models;

namespace QuickReach.ECommerce.Infra.Data.EntityConfiguration
{
    class ProductManufacturerEntityTypeConfiguration : IEntityTypeConfiguration<ProductManufacturer>
    {
        public void Configure(EntityTypeBuilder<ProductManufacturer> builder)
        {
            builder.ToTable("ProductManufacturer");
            builder.HasKey(cr => new { cr.ManufacturerId, cr.ProductId });
            builder.HasOne(cr => cr.Manufacturer)
                   .WithMany(c => c.ProductManufacturers)
                   .HasForeignKey("ManufacturerId");

            builder.HasOne(cr => cr.Product)
                   .WithMany(c => c.ProductManufacturers)
                   .HasForeignKey("ProductId");
        }
    }


}

