using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace QuickReach.ECommerce.Infra.Data.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Create_WithValidEntity_ShouldCreateDatabaseRecord()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };
            //Act
            sut.Create(category);
            //Assert
            Assert.True(category.ID != 0);

            var entity = sut.Retrieve(category.ID);
            Assert.NotNull(entity);

            //Cleanup
            sut.Delete(category.ID);
        }

        [Fact]
        public void Retrieve_WithValidEntityID_ReturnsAValidEntity()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };
            sut.Create(category);
            //Act
            var actual = sut.Retrieve(category.ID);
            //Assert
            Assert.NotNull(actual);
            //Cleanup
            sut.Delete(actual.ID);

        }

        [Fact]
        public void Retrieve_WithNonExistentEntityID_ReturnsNull()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);
            //Act
            var actual = sut.Retrieve(-1);
            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void Retrieve_WithSkipAndCount_ReturnsTheCorrectPage()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);
            for (var i = 1; i <= 20; i += 1)
            {
                sut.Create(new Category
                {
                    Name = string.Format("Category {0}", i),
                    Description = string.Format("Description {0}", i)
                });
            }
            //Act
            var list = sut.Retrieve(5, 5).ToList();
            //Assert
            Assert.True(list.Count() == 5);
            //Cleanup
            foreach(Category cat in list)
            {
                sut.Delete(cat.ID);
            }
        }

        [Fact]
        public void Delete_WithValidEntity_ShouldRemoveAddedRecords()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };
            sut.Create(category);
            //Act
            var actual = sut.Retrieve(category.ID);
            sut.Delete(actual.ID);

            //Assert
            Assert.Null(sut.Retrieve(actual.ID));
            //Cleanup
            
        }


        [Fact]
        public void Update_WithValidEntity_ShouldUpdateRecords()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };
            sut.Create(category);

            var actual = sut.Retrieve(category.ID);
            var expectedName = "Shoepatos";
            var expectedDescription = "Sapatos natin";

            actual.Name = expectedName;
            actual.Description = expectedDescription;
            //Act
            sut.Update(actual.ID, actual);

            //Assert
            var expected = sut.Retrieve(actual.ID);

            Assert.Equal(expectedName, expected.Name);
            Assert.Equal(expectedDescription, expected.Description);
            //Cleanup
            sut.Delete(actual.ID);
        }
    }
}
