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
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        public ProductsController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Get(string search = "", int skip = 0, int count = 100)
        {
            var products = this.productRepository.Retrieve(search, skip, count);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = this.productRepository.Retrieve(id);
            return Ok(product);
        }

        [HttpGet("{id}/categories")]
        public IActionResult GetCategories(int id)
        {
            var product = this.productRepository.Retrieve(id);
            return Ok(product.ProductCategories);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product newProduct)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.productRepository.Create(newProduct);

            return CreatedAtAction(nameof(this.Get), new { id = newProduct.ID }, newProduct);
        }

        [HttpPost("{id}")]
        //public IActionResult Post([FromBody] Product newProduct)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    this.productRepository.Create(newProduct);

        //    return CreatedAtAction(nameof(this.Get), new { id = newProduct.ID }, newProduct);
        //}

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            this.productRepository.Update(id, product);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.productRepository.Delete(id);

            return Ok();
        }
    }
}