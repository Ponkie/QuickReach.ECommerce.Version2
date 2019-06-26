using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using System;
using System.Linq;
using Xunit;

namespace QuickReach.ECommerce.Infra.Data.Test
{
    public class ProductRepositoryTest
    {
        [Fact]
        public void Create_WithValidEntity_ShouldCreateDatabaseRecord()
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
                Name = "Shoes",
                Description = "Running, Leather & Boots"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Categories.Add(category);
                context.SaveChanges();
            }

            var expected = new Product
            {
                Name = "UltraBoost 4.0",
                Description = "Legend Ink",
                Price = 1500,
                CategoryID = category.ID,
                ImageUrl = "https://static1.squarespace.com/static/532313ece4b08487acaec7a2/t/5a58c33171c10baff724264e/1515766581481/DTWLHKxWAAA84qZ.jpg?",
                IsActive = true
            };

            using (var context = new ECommerceDbContext(options))
            {
                //Act
                var sut = new ProductRepository(context);
                sut.Create(expected);
            }

            using (var context = new ECommerceDbContext(options))
            {
                //Assert
                var actual = context.Products.Find(expected.ID);

                Assert.NotNull(actual);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Description, actual.Description);
            }

        }

        [Fact]
        public void Retrieve_WithExistentEntityID_ShouldGetRecord()
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
                Name = "Clothing",
                Description = "Shirts, Polo, and Dress"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Categories.Add(category);
                context.SaveChanges();
            }

            var product = new Product
            {
                Name = "White T-Shirt",
                Description = "Small",
                Price = 250,
                CategoryID = category.ID,
                ImageUrl = "https://de9luwq5d40h2.cloudfront.net/catalog/product/zoom_image/00_407044.jpg",
                IsActive = true
            };


            using (var context = new ECommerceDbContext(options))
            {
                context.Products.Add(product);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                //Act
                var sut = new ProductRepository(context);
                var actual = sut.Retrieve(product.ID);
                //Assert
                Assert.NotNull(actual);
            }

        }


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

            var category = new Category
            {
                Name = "Phone",
                Description = "Smartphone, Flip Phones"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Categories.Add(category);
                context.SaveChanges();
            }

            var product = new Product
            {
                Name = "Redmi Note 7",
                Description = "Blue 6GB RAM 128GB Storage",
                Price = 100,
                CategoryID = category.ID,
                ImageUrl = "https://d2pa5gi5n2e1an.cloudfront.net/global/images/product/mobilephones/Xiaomi_Redmi_Note_7/Xiaomi_Redmi_Note_7_L_1.jpg",
                IsActive = true
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Products.Add(product);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                //Act
                var sut = new ProductRepository(context);
                sut.Delete(product.ID);

                //Assert
                Assert.Null(context.Products.Find(product.ID));
            }
        }

        [Fact]
        public void Retrieve_WithPagination_ReturnsCorrectPage()
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
                Description = "Analog & Digital Watches"
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
                for (int i = 1; i <= 20; i += 1)
                {
                    context.Products.Add(new Product
                    {
                        Name = string.Format("Name {0}", i),
                        Description = string.Format("Description {0}", i),
                        Price = i,
                        CategoryID = category.ID,
                        ImageUrl = string.Format("Image{0}.jpg", i),
                        IsActive = true
                    });
                }
                context.SaveChanges();
            }
            using (var context = new ECommerceDbContext(options))
            {
                //Act
                var sut = new ProductRepository(context);
                var list = sut.Retrieve(5, 5);
                //Assert
                Assert.True(list.Count() == 5);
            }
        }

        [Fact]
        public void Update_WithValidEntityID_ShouldUpdateRecord()
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
                Name = "Shoes",
                Description = "Running, Leather & Boots"
            };

            var category2 = new Category
            {
                Name = "Clothing",
                Description = "Shirts, Long-Sleeves & Dress"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Categories.Add(category);
                context.Categories.Add(category2);
                context.SaveChanges();
            }

            var product = new Product
            {
                Name = "UltraBoost 4.0",
                Description = "Legend Ink",
                Price = 1500,
                CategoryID = category.ID,
                ImageUrl = "https://static1.squarespace.com/static/532313ece4b08487acaec7a2/t/5a58c33171c10baff724264e/1515766581481/DTWLHKxWAAA84qZ.jpg?",
                IsActive = true
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Products.Add(product);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var actual = context.Products.Find(product.ID);
                var expectedName = "T-Shirt";
                var expectedDesc = "Cotton";
                var expectedPrice = 150;
                var expectedCategory = category2.ID;
                var expectedImg = "https://de9luwq5d40h2.cloudfront.net/catalog/product/zoom_image/00_407044.jpg";

                actual.Name = expectedName;
                actual.CategoryID = expectedCategory;
                actual.Description = expectedDesc;
                actual.Price = expectedPrice;
                actual.ImageUrl = expectedImg;

                //Act
                var sut = new ProductRepository(context);
                sut.Update(actual.ID, actual);

                //Assert
                var expected = context.Products.Find(product.ID);

                Assert.Equal(expected.Name, expectedName);
                Assert.Equal(expected.Description, expectedDesc);
                Assert.Equal(expected.Price, expectedPrice);
                Assert.Equal(expected.CategoryID, expectedCategory);
                Assert.Equal(expected.ImageUrl, expectedImg);

            }

        }

        [Fact]
        public void Create_WithInvalidCategoryID_ShouldThrowException()
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

            var product = new Product
            {
                Name = "UltraBoost 4.0",
                Description = "Legend Ink",
                Price = 1500,
                CategoryID = 0,
                ImageUrl = "https://static1.squarespace.com/static/532313ece4b08487acaec7a2/t/5a58c33171c10baff724264e/1515766581481/DTWLHKxWAAA84qZ.jpg?",
                IsActive = true
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                //Act//Actual
                var sut = new ProductRepository(context);
                Assert.Throws<SystemException>( ()=> sut.Create(product));
            }



        }

    }
}
