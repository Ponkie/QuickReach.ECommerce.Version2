using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.API.ViewModel;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data;
using QuickReach.ECommerce.Infra.Data.Repositories;
using Dapper;

namespace QuickReach.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ECommerceDbContext context;
        public CategoriesController(ICategoryRepository categoryRepository, IProductRepository productRepository, ECommerceDbContext context)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Get(string search="", int skip = 0, int count = 10)
        {
            var categories = this.categoryRepository.Retrieve(search, skip, count);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = this.categoryRepository.Retrieve(id);
            return Ok(category);
        }

        [HttpGet("{id}/products")]
        public IActionResult GetProductsByCategory(int id)
        {
                var connectionString =
                "Server=.;Database=QuickReachDb;Integrated Security=true;";
                var connection = new SqlConnection(connectionString);
                var sql = @"SELECT p.ID,
                         pc.ProductId, 
                         pc.CategoryId,
                         p.Name, 
                         p.Description,
                         p.Price,
                         p.ImageUrl
                    FROM Product p INNER JOIN ProductCategory pc ON p.ID = pc.ProductId
                    Where pc.CategoryId = @categoryId";
                var categories = connection
                    .Query<SearchItemViewModel>(
                    sql, new { categoryId = id })
                        .ToList();
                return Ok(categories);

        }


        [HttpPost]
        public IActionResult Post([FromBody] Category newCategory)
        {
            if(!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.categoryRepository.Create(newCategory);

            return CreatedAtAction(nameof(this.Get), new { id = newCategory.ID }, newCategory);
        }

        [HttpPut("{id}/products")]
        public IActionResult AddCategoryProduct(int id, [FromBody] ProductCategory entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var category = categoryRepository.Retrieve(id);
            if (category == null)
            {
                return NotFound();
            }
            var product = productRepository.Retrieve(entity.ProductId);
            if (product == null)
            {
                return NotFound();
            }
            category.AddProduct(entity);
            product.AddCategory(entity);

            categoryRepository.Update(entity.CategoryId, category);
            productRepository.Update(entity.ProductId, product);
            return Ok(category);

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.categoryRepository.Delete(id);

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Category newCategory)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.categoryRepository.Update(id, newCategory);

            return Ok(newCategory);
        }

        [HttpDelete("{id}/products/{productId}")]
        public IActionResult DeleteCategoryProduct(int id, int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var category = categoryRepository.Retrieve(id);
            if (category == null)
            {
                return NotFound();
            }
            if (productRepository.Retrieve(productId) == null)
            {
                return NotFound();
            }
            category.RemoveProduct(productId);
            categoryRepository.Update(id, category);
            return Ok();
        }
    }
}