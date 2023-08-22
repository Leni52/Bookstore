using Bookstore.Application.Common.Interfaces;
using Bookstore.Application.Requests.Queries.FetchBooks;
using Bookstore.Domain.Common;
using Bookstore.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.CommandsBooks
{
    public class UpdateBookCommand : IRequest<Book>
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public GenreType Genre { get; set; }
        public int YearOfPublishing { get; set; }
        public Guid AuthorId { get; set; }
        public UpdateBookCommand(string title, string description, int yearOfPublishing, Guid authorId, GenreType genre)
        {
            Title = title;
            Description = description;
            YearOfPublishing = yearOfPublishing;
            AuthorId = authorId;
            Genre = genre;
        }
        public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Book>
        {
            private readonly IBookStoreContext context;
            public UpdateBookCommandHandler(IBookStoreContext context)
            {
                this.context = context;
            }

            public async Task<Book> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
            {
                var bookToUpdate = await context.Books.Where(b => b.Id == command.BookId).FirstOrDefaultAsync();
                if (bookToUpdate == null)
                {
                    throw new ArgumentNullException(nameof(bookToUpdate));
                }
                bookToUpdate.Title = command.Title;
                bookToUpdate.Description = command.Description;
                bookToUpdate.Genre = command.Genre;
                bookToUpdate.YearOfPublishing = command.YearOfPublishing;
                bookToUpdate.AuthorId = command.AuthorId;
                bookToUpdate.ModifiedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
                return bookToUpdate;
            }
        }
    }
}
