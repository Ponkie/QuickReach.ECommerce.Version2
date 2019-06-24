using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using System;
using System.Collections.Generic;
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
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var product = new Product
            {
                Name = "Shoes",
                Description = "Shoes Department",
                Price = 100,
                CategoryID = 1,
                ImageUrl = "image.jpg",
                IsActive = true
            };
            //Act
            sut.Create(product);
            //Assert
            Assert.True(product.ID != 0);

            var entity = sut.Retrieve(product.ID);
            Assert.NotNull(entity);

            //Cleanup
            sut.Delete(product.ID);
        }
    }

