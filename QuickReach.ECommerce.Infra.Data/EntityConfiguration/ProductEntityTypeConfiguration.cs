using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace QuickReach.ECommerce.Infra.Data.EntityConfiguration
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public object CategoryID { get; internal set; }

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.ID)
                .IsRequired()
                .ValueGeneratedOnAdd();
        }
    }
}
