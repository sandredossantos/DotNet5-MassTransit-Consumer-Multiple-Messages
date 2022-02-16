using Identity.Api.Models;
using Identity.Contracts.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IBus _bus;

        public UsersController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            await _bus.Publish<AddUser>(new { user.Id }, x => x.SetRoutingKey("add:key"));

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            await _bus.Publish<DeleteUser>(new { Id = id }, x => x.SetRoutingKey("delete:key"));

            return Ok();
        }
    }
}