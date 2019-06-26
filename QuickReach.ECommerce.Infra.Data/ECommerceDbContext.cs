using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;

namespace QuickReach.ECommerce.Infra.Data
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(
            DbContextOptions<ECommerceDbContext> options) : base(options)
        {

        }

        public ECommerceDbContext() : base()
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var connectionString = "Server=.;Database=QuickReachDb;Integrated Security=true;";
            //optionsBuilder.UseSqlServer(connectionString);
            //base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}
