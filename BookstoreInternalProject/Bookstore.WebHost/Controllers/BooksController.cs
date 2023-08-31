﻿using Bookstore.Application.Requests.Commands.CommandsBooks;
using Bookstore.Application.Requests.Queries.FetchBooks;
using Bookstore.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator mediator;
        public BooksController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet()]
        public async Task<IActionResult> FetchAllBooks()
        {
            return Ok(await this.mediator.Send(new FetchAllBooksQuery()));
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] AddBookCommand command)
        {
            return Ok(await this.mediator.Send(command));
        }

        [HttpDelete("bookId")]
        public async Task<IActionResult> DeleteBookById(Guid bookId)
        {
            var command = new DeleteBookByIdCommand(bookId);
            return Ok(await this.mediator.Send(command));
        }

        [HttpDelete("title")]
        public async Task<IActionResult> DeleteBookByTitle(string bookTitle)
        {
            var command = new DeleteBookByTitleCommand(bookTitle);
            return Ok(await this.mediator.Send(command));
        }
        [HttpPut("bookId")]
        public async Task<IActionResult> UpdateBookById(Guid bookId, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var command = new UpdateBookCommand(bookId, book.Title, book.Description, book.YearOfPublishing, book.AuthorId, book.Genre);
            var result = await mediator.Send(command);
            return Ok(result);
        }

    }
}
