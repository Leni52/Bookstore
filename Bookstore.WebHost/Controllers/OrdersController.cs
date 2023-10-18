using Bookstore.Application.Requests.Commands.CommandsCustomers;
using Bookstore.Application.Requests.Queries.FetchOrders;
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

        [HttpGet()]
        public async Task<IActionResult> FetchAllOrders()
        {
            return Ok(await this.mediator.Send(new FetchAllOrdersQuery()));
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] MakeOrderCommand command)
        {
            return Ok(await this.mediator.Send(command));
        }
    }
}
