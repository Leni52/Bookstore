using Bookstore.Application.Requests.Commands.CommandsAuthors;
using Bookstore.Application.Requests.Queries.FetchAuthors;
using Bookstore.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IMediator mediator;
        public AuthorsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet()]
        public async Task<IActionResult> FetchAllAuthors()
        {
            return Ok(await this.mediator.Send(new FetchAllAuthorsQuery()));
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromBody] AddAuthorCommand command)
        {
            return Ok(await this.mediator.Send(command));
        }

        /*
        [HttpDelete("authorId")]
        public async Task<IActionResult> DeleteAuthorById(Guid authorId)
        {
            var command = new DeleteAuthorByIdCommand(authorId);
            return Ok(await this.mediator.Send(command));
        }
        */
        [HttpPut("authorId")]
        public async Task<IActionResult> UpdateBookById(Guid authorId, Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var command = new UpdateAuthorCommand(author.Name, author.Biography);
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}
