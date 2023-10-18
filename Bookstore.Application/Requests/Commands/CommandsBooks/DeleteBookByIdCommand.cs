using Bookstore.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.Commands.CommandsBooks
{
    public class DeleteBookByIdCommand : IRequest<Guid>
    {
        public Guid BookId { get; set; }
        public DeleteBookByIdCommand(Guid bookid)
        {
            BookId = bookid;
        }
        public class DeleteBookByIdHandler : IRequestHandler<DeleteBookByIdCommand, Guid>
        {
            private readonly IBookStoreContext context;
            private readonly IValidator<DeleteBookByIdCommand> validator;
            public DeleteBookByIdHandler(IBookStoreContext context, IValidator<DeleteBookByIdCommand> validator)
            {
                this.context = context;
                this.validator = validator;
            }
            public async Task<Guid> Handle(DeleteBookByIdCommand command, CancellationToken cancellationToken)
            {
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var book = context.Books
        .FirstOrDefault(b => b.Id == command.BookId);

                if (book == null)
                {
                    throw new ArgumentNullException($"Book with ID {command.BookId} has not been found.");
                }

                book.Quantity = book.Quantity > 0 ? book.Quantity - 1 : 0;

                context.Books.Remove(book);
                await context.SaveChangesAsync();
                return book.Id;
            }
        }
    }
}
