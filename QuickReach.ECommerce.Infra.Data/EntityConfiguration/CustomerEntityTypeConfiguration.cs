using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace QuickReach.ECommerce.Infra.Data.EntityConfiguration
{
    class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {

        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.ID)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.HasMany(c => c.Carts);
        }
    }
}