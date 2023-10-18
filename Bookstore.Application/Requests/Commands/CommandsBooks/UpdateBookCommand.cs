using Bookstore.Application.Common.Interfaces;
using Bookstore.Domain.Common;
using Bookstore.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.Commands.CommandsBooks
{
    public class UpdateBookCommand : IRequest<Book>
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public GenreType Genre { get; set; }
        public int YearOfPublishing { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Guid AuthorId { get; set; }
        public UpdateBookCommand(Guid bookId, string title, string description, int yearOfPublishing, double price, int quantity, Guid authorId, GenreType genre)
        {
            BookId = bookId;
            Title = title;
            Description = description;
            YearOfPublishing = yearOfPublishing;
            Price = price;
            Quantity = quantity;
            AuthorId = authorId;
            Genre = genre;
        }
        public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Book>
        {
            private readonly IBookStoreContext context;
            private readonly IValidator<UpdateBookCommand> validator;
            public UpdateBookCommandHandler(IBookStoreContext context, IValidator<UpdateBookCommand> validator)
            {
                this.context = context;
                this.validator = validator;
            }

            public async Task<Book> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
            {
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var bookToUpdate = context.Books.FirstOrDefault(b => b.Id == command.BookId);

                if (bookToUpdate == null)
                {
                    throw new ArgumentNullException(nameof(bookToUpdate));
                }

                bookToUpdate.Title = command.Title ?? bookToUpdate.Title;
                bookToUpdate.Description = command.Description ?? bookToUpdate.Description;
                bookToUpdate.Genre = command.Genre == 0 ? bookToUpdate.Genre : command.Genre;
                bookToUpdate.YearOfPublishing = command.YearOfPublishing == 0 ? bookToUpdate.YearOfPublishing : command.YearOfPublishing;
                bookToUpdate.Quantity = command.Quantity == 0 ? bookToUpdate.Quantity : command.Quantity;
                bookToUpdate.Price = command.Price == 0 ? bookToUpdate.Price : command.Price;
                bookToUpdate.AuthorId = command.AuthorId == Guid.Empty ? bookToUpdate.AuthorId : command.AuthorId;
                bookToUpdate.ModifiedAt = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);
                return bookToUpdate;
            }
        }
    }
}
