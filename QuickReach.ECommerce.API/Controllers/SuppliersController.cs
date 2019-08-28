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
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository repository;
        private readonly IProductRepository productRepository;
        private readonly ECommerceDbContext context;
        public SuppliersController(ISupplierRepository repository, IProductRepository productRepository, ECommerceDbContext context)
        {
            this.repository = repository;
            this.productRepository = productRepository;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Get(string search = "", int skip = 0, int count = 10)
        {
            var supplier = repository.Retrieve(search, skip, count);
            return Ok(supplier);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var supplier = this.repository.Retrieve(id);
            return Ok(supplier);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Supplier newSupplier)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Create(newSupplier);

            return CreatedAtAction(nameof(this.Get), new { id = newSupplier }, newSupplier);
        }

        [HttpPut("{id}/products")]
        public IActionResult AddSupplierProduct(int id, [FromBody] ProductSupplier entity)
        {
            var supplier = repository.Retrieve(id);
            var product = productRepository.Retrieve(entity.ProductID);

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

            supplier.AddProduct(entity);
            repository.Update(id, supplier);

            return Ok(supplier);
        }

        [HttpPut("{id}/products/{productId}")]
        public IActionResult DeleteSupplierProduct(int id, int productId)
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

            supplier.RemoveProduct(productId);
            repository.Update(id, supplier);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Supplier newSupplier)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Update(id, newSupplier);

            return Ok(newSupplier);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.repository.Delete(id);
            return Ok();
        }

    }
}