using Bookstore.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.Queries.FetchBooks
{
    public class FetchAllBooksQuery : IRequest<IEnumerable<BookDto>>
    {
        public class FetchAllBooksQueryHandler : IRequestHandler<FetchAllBooksQuery, IEnumerable<BookDto>>
        {
            private readonly IBookStoreContext context;

            public FetchAllBooksQueryHandler(IBookStoreContext context)
            {
                this.context = context;
            }

            public async Task<IEnumerable<BookDto>> Handle(FetchAllBooksQuery request, CancellationToken cancellationToken)
            {
                var books = await this.context
                    .Books
                    .Include(b => b.Author)
                    .Select(b => new BookDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Description = b.Description,
                        Genre = b.Genre,
                        AuthorName = b.Author.Name
                    })
                    .ToListAsync();

                return books;
            }
        }
    }
}
