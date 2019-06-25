using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace QuickReach.ECommerce.Infra.Data.Test
{
    public class SupplierRepositoryTest
    {
        [Fact]
        public void Create_WithValidEntity_ShouldCreateDatabaseRecord()
        {
            // Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options;


            var expected = new Supplier
            {
                Name = "Converse",
                Description = "All-Star Shoe Distributor",
                IsActive = true
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var sut = new SupplierRepository(context);

                // Act
                sut.Create(expected);
            }

            using (var context = new ECommerceDbContext(options))
            {
                // Assert
                var actual = context.Suppliers.Find(expected.ID);

                Assert.NotNull(actual);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Description, actual.Description);

            }
        }

        [Fact]
        public void Retrieve_WithExistingEntityID_ShouldGetRecords()
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options;

            var supplier = new Supplier
            {
                Name = "Converse",
                Description = "All-Star Shoe Distributor",
                IsActive = true
            };
            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Suppliers.Add(supplier);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                //Act
                var sut = new SupplierRepository(context);
                var actual = sut.Retrieve(supplier.ID);
                //Assert
                Assert.NotNull(actual);
            }
        }
        //[Fact]
        //public void Retrieve_WithNonExistentEntityID_ReturnsNull()
        //{
        //    //Arrange
        //    var context = new ECommerceDbContext();
        //    var sut = new SupplierRepository(context);
        //    //Act
        //    var actual = sut.Retrieve(-1);
        //    //Assert
        //    Assert.Null(actual);
        //}

        [Fact]
        public void Delete_WithExistentEntityID_ShouldDeleteRecord()
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };

            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                .UseSqlite(connection)
                .Options;

            var supplier = new Supplier
            {
                Name = "CDR-King",
                Description = "All-Around Seller",
                IsActive = true
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Suppliers.Add(supplier);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var actual = context.Suppliers.Find(supplier.ID);
                //Act
                var sut = new SupplierRepository(context);
                sut.Delete(actual.ID);
                //Assert
                Assert.Null(context.Suppliers.Find(actual.ID));
            }
        }

        [Fact]
        public void Retrieve_WithPagination_ReturnsTheCorrectPage()
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };

            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                for (int i = 1; i <= 20; i += 1)
                {
                    context.Suppliers.Add(new Supplier
                    {
                        Name = string.Format("Supplier {0}", i),
                        Description = string.Format("Description {0}", i),
                        IsActive = true
                    });
                }
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                //Act
                var sut = new SupplierRepository(context);
                var list = sut.Retrieve(5, 5);
                //Assert
                Assert.True(list.Count() == 5);
            }
        }
        [Fact]
        public void Update_WithExistentEntityID_ShouldUpdateRecord()
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder();

            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                .UseSqlite(connection)
                .Options;

            var supplier = new Supplier
            {
                Name = "Apple",
                Description = "Apple Phones & Other Products",
                IsActive = true
            };

            var expectedName = "Samsung";
            var expectedDesc = "Samsung Phones & Tablets";

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Suppliers.Add(supplier);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var actual = context.Suppliers.Find(supplier.ID);
                actual.Name = expectedName;
                actual.Description = expectedDesc;
                //Act
                var sut = new SupplierRepository(context);
                sut.Update(actual.ID, actual);
                //Assert
                var expected = context.Suppliers.Find(supplier.ID);
                Assert.Equal(expected.Name, expectedName);
                Assert.Equal(expected.Description, expectedDesc);
            }
        }
    }
}
