using Bookstore.Application.Requests.Commands.CommandsCustomers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator mediator;
        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }



        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] MakeOrderCommand command)
        {
            return Ok(await this.mediator.Send(command));
        }
    }
}
