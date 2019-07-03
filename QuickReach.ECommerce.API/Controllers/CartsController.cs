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


        [HttpGet("{id}/items")]
        public IActionResult GetCartItemsInCart(int id)
        {
            var cart = this.cartRepository.Retrieve(id);
            return Ok(cart);
        }

        [HttpPut("{id}/items")]
        public IActionResult AddCartItems(int id, [FromBody] CartItem entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var cart = cartRepository.Retrieve(id);

            if (cart == null)
            {
                return NotFound();
            }

            if (productRepository.Retrieve(entity.ProductId) == null)
            {
                return NotFound();
            }

            cart.AddCartItem(entity);

            cartRepository.Update(entity.Id, cart);
            return Ok(cart);

        }

    }
}