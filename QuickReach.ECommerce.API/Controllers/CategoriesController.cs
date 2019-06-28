using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data;
using QuickReach.ECommerce.Infra.Data.Repositories;

namespace QuickReach.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
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
            if (productRepository.Retrieve(entity.ProductID) == null)
            {
                return NotFound();
            }
            category.AddProduct(entity);

            categoryRepository.Update(id, category);
            return Ok(category);

        }

        [HttpPut]
        public IActionResult Put(int id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            this.categoryRepository.Update(id, category);

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.categoryRepository.Delete(id);

            return Ok();
        }

        [HttpPut("{id}/products/{productId}")]
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