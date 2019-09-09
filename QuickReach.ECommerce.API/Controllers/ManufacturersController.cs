using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data;

namespace QuickReach.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        private readonly IManufacturerRepository repository;
        private readonly IProductRepository productRepository;
        private readonly ECommerceDbContext context;
        public ManufacturersController(IManufacturerRepository repository, IProductRepository productRepository, ECommerceDbContext context)
        {
            this.repository = repository;
            this.productRepository = productRepository;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Get(string search = "", int skip = 0, int count = 10)
        {
            var manufacturer = repository.Retrieve(search, skip, count);
            return Ok(manufacturer);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var manufacturer = this.repository.Retrieve(id);
            return Ok(manufacturer);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Manufacturer newManufacturer)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Create(newManufacturer);

            return CreatedAtAction(nameof(this.Get), new { id = newManufacturer }, newManufacturer);
        }

        [HttpPut("{id}/products")]
        public IActionResult AddSupplierManufacturer(int id, [FromBody] ProductManufacturer entity)
        {
            var manufacturer = repository.Retrieve(id);
            var product = productRepository.Retrieve(entity.ProductId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (manufacturer == null)
            {
                return NotFound();
            }

            if (product == null)
            {
                return NotFound();
            }

            manufacturer.AddProduct(entity);
            repository.Update(id, manufacturer);

            return Ok(manufacturer);
        }

        [HttpPut("{id}/products/{productId}")]
        public IActionResult DeleteSupplierManufacturer(int id, int productId)
        {
            var supplier = repository.Retrieve(id);
            var product = productRepository.Retrieve(productId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (supplier == null)
            {
                return NotFound();
            }
            if (product == null)
            {
                return NotFound();
            }
           
            repository.Update(id, supplier);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Manufacturer newManufacturer)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Update(id, newManufacturer);

            return Ok(repository);
        }



        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.repository.Delete(id);
            return Ok();
        }

    }
}