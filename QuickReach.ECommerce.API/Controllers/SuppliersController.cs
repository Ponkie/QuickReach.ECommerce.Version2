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
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository supplierRepository;
        private readonly IProductRepository productRepository;
        private readonly ECommerceDbContext context;
        public SuppliersController(ISupplierRepository supplierRepository, IProductRepository productRepository, ECommerceDbContext context)
        {
            this.supplierRepository = supplierRepository;
            this.productRepository = productRepository;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Get(string search = "", int skip = 0, int count = 10)
        {
            var suppliers = this.supplierRepository.Retrieve(search, skip, count);
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var supplier = this.supplierRepository.Retrieve(id);
            return Ok(supplier);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Supplier newSupplier)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.supplierRepository.Create(newSupplier);

            return CreatedAtAction(nameof(this.Get), new { id = newSupplier.ID }, newSupplier);
        }

        [HttpPut]
        public IActionResult Put(int id, [FromBody] Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            this.supplierRepository.Update(id, supplier);

            return Ok(supplier);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.supplierRepository.Delete(id);

            return Ok();
        }

        [HttpPut("{id}/products")]
        public IActionResult AddProductSupplier(int id, [FromBody] ProductSupplier entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var supplier = supplierRepository.Retrieve(id);
            if (supplier == null)
            {
                return NotFound();
            }
            if (productRepository.Retrieve(entity.ProductID) == null)
            {
                return NotFound();
            }

            supplier.AddProduct(entity);

            supplierRepository.Update(id, supplier);
            return Ok(supplier);

        }
    }
}