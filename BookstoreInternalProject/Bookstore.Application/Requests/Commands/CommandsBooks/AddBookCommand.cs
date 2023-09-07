using Bookstore.Application.Common.Interfaces;
using Bookstore.Domain.Common;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.Commands.CommandsBooks
{
    public class AddBookCommand : IRequest<bool>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int YearOfPublishing { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public GenreType Genre { get; set; }
        public string AuthorName { get; set; }
        public class AddBookCommandHandler : IRequestHandler<AddBookCommand, bool>
        {
            private readonly IBookStoreContext context;
            private readonly IValidator<AddBookCommand> validator;
            public AddBookCommandHandler(IBookStoreContext context, IValidator<AddBookCommand> validator)
            {
                this.context = context;
                this.validator = validator;
            }

            public async Task<bool> Handle(AddBookCommand command, CancellationToken cancellationToken)
            {
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
                try
                {
                    var author = await context.Authors.FirstOrDefaultAsync(a => a.Name == command.AuthorName);
                    if (author == null)
                    {
                        author = new Domain.Entities.Author { Name = command.AuthorName };
                        context.Authors.Add(author);
                        await context.SaveChangesAsync();
                    }
                    context.Books.Add(new Domain.Entities.Book
                    {
                        Title = command.Title,
                        Description = command.Description,
                        Genre = command.Genre,
                        YearOfPublishing = command.YearOfPublishing,
                        Price = command.Price,
                        Quantity = command.Quantity,
                        AuthorId = author.Id,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow
                    });

                    await context.SaveChangesAsync();
                    return true;
                }

                catch (Exception)
                {

                    return false;
                }
            }
        }
    }
}
