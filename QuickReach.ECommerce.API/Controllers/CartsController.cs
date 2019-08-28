using System;
using System.Collections.Generic;
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
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository cartRepository;
        private readonly IProductRepository productRepository;
        private readonly ECommerceDbContext context;
        public CartsController(ICartRepository cartRepository, IProductRepository productRepository, ECommerceDbContext context)
        {
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Get(string search = "", int skip = 0, int count = 10)
        {
            var carts = this.cartRepository.Retrieve(search, skip, count);
            return Ok(carts);
        }

        [HttpGet("{id}/items")]
        public IActionResult GetCartItemsInCart(int id)
        {
            var cart = cartRepository.Retrieve(id);
            return Ok(cart);
        }

        [HttpPost("{id}/items/{productId}")]
        public IActionResult AddCartItems(int id, int productId, [FromBody] CartItem entity)
        {
            var cart = cartRepository.Retrieve(id);
            var product = productRepository.Retrieve(productId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            if (cart == null)
            {
                return NotFound();
            }

            if (product == null)
            {
                return NotFound();
            }

            cart.AddCartItem(product, entity);

            cartRepository.Update(entity.Id, cart);
            return Ok(cart);

        }

    }
}