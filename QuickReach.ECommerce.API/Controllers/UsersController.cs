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
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository repository;
        private readonly ECommerceDbContext context;
        public UsersController(IUserRepository repository, ECommerceDbContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Get(string search = "", int skip = 0, int count = 10)
        {
            var user = repository.Retrieve(search, skip, count);
            return Ok(user);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = this.repository.Retrieve(id);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User newUser)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Create(newUser);

            return CreatedAtAction(nameof(this.Get), new { id = newUser }, newUser);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User newUser)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Update(id, newUser);

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