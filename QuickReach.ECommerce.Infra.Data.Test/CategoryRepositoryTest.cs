using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace QuickReach.ECommerce.Infra.Data.Test
{
    public class CategoryRepositoryTest
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


            var expected = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var sut = new CategoryRepository(context);

                // Act
                sut.Create(expected);
            }

            using (var context = new ECommerceDbContext(options))
            {
                // Assert
                var actual = context.Categories.Find(expected.ID);

                Assert.NotNull(actual);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Description, actual.Description);

            }
        }

        //[Fact]
        //public void Create_WithValidEntity_ShouldCreateDatabasRecord()
        //{
        //    //Arrange
        //    var context = new ECommerceDbContext();
        //    var sut = new CategoryRepository(context);
        //    var category = new Category
        //    {
        //        Name = "Shoes",
        //        Description = "Shoes Department"
        //    };
        //    //Act
        //    sut.Create(category);
        //    //Assert
        //    Assert.True(category.ID != 0);

        //    var entity = sut.Retrieve(category.ID);
        //    Assert.NotNull(entity);

        //    //Cleanup
        //    sut.Delete(category.ID);
        //}

        [Fact]
        public void Retrieve_WithValidEntityID_ReturnsAValidEntity()
        {
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options;


            var expected = new Category
            {
                Name = "Watch",
                Description = "Watch Department"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Categories.Add(expected);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                //Act
                var sut = new CategoryRepository(context);
                var actual = sut.Retrieve(expected.ID);
                // Assert
                Assert.NotNull(actual);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Description, actual.Description);
            }
        }

        //[Fact]
        //public void Retrieve_WithNonExistentEntityID_ReturnsNull()
        //{
        //    //Arrange
        //    var context = new ECommerceDbContext();
        //    var sut = new CategoryRepository(context);
        //    //Act
        //    var actual = sut.Retrieve(-1);
        //    //Assert
        //    Assert.Null(actual);
        //}

        [Fact]
        public void Retrieve_WithSkipAndCount_ReturnsTheCorrectPage()
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
                for (var i = 1; i <= 20; i += 1)
                {
                    context.Categories.Add(new Category
                    {
                        Name = string.Format("Category {0}", i),
                        Description = string.Format("Description {0}", i)
                    });
                }
                context.SaveChanges();
            }
            
            using (var context = new ECommerceDbContext(options))
            {
                //Act
                var sut = new CategoryRepository(context);
                var actual = sut.Retrieve(5, 5);
                //Assert
                Assert.True(actual.Count()==5);
            }
        }

        [Fact]
        public void Delete_WithValidEntity_ShouldRemoveAddedRecords()
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

            var category = new Category
            {
                Name = "Watch",
                Description = "Watch Department"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Categories.Add(category);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var actual = context.Categories.Find(category.ID);
                //Act
                var sut = new CategoryRepository(context);
                sut.Delete(actual.ID);

                //Assert
                Assert.Null(context.Categories.Find(actual.ID));
            }
        }


        [Fact]
        public void Update_WithValidEntity_ShouldUpdateRecords()
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

            var category = new Category
            {
                Name = "Watch",
                Description = "Watch Department"
            };

            var expectedName = "Shoes";
            var expectedDesc = "Shoe Department";

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Categories.Add(category);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var actual = context.Categories.Find(category.ID);
                actual.Name = expectedName;
                actual.Description = expectedDesc;

                //Act
                var sut = new CategoryRepository(context);
                sut.Update(actual.ID, actual);

                //Assert
                var expected = context.Categories.Find(actual.ID);
                Assert.Equal(expectedName, expected.Name);
                Assert.Equal(expectedDesc, expected.Description);
            }
        }
    }
}
