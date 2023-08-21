using Bookstore.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.CommandsBooks
{
    public class DeleteBookByTitleCommand : IRequest<int>
    {
        public string Title { get; set; }
        public DeleteBookByTitleCommand(string title)
        {
            Title = title;
        }
        public class DeleteBookByTitleHandler : IRequestHandler<DeleteBookByTitleCommand, int>
        {
            private readonly IBookStoreContext context;
            public DeleteBookByTitleHandler(IBookStoreContext context)
            {
                this.context = context;
            }
            public async Task<int> Handle(DeleteBookByTitleCommand request, CancellationToken cancellationToken)
            {
                var booksToRemove = await context
                     .Books
                     .Where(b => b.Title == request.Title)
                     .ToListAsync();
                if (booksToRemove == null || !booksToRemove.Any())
                {
                    throw new ArgumentNullException(nameof(booksToRemove));
                }
                context.Books.RemoveRange(booksToRemove);
                await context.SaveChangesAsync();
                return booksToRemove.Count;
            }

        }
    }
}
