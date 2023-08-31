using Bookstore.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.Queries.FetchBooks
{
    public class FetchBooksByYearQuery : IRequest<IEnumerable<BookDto>>
    {
        public int YearOfPublishing { get; set; }
        public class FetchBooksByYearQueryHandler : IRequestHandler<FetchBooksByYearQuery, IEnumerable<BookDto>>
        {
            private readonly IBookStoreContext context;

            public FetchBooksByYearQueryHandler(IBookStoreContext context)
            {
                this.context = context;
            }

            public async Task<IEnumerable<BookDto>> Handle(FetchBooksByYearQuery request, CancellationToken cancellationToken)
            {
                var year = request.YearOfPublishing;
                var books = await this.context
                    .Books
                    .Include(b => b.Author)
                     .Where(b => b.YearOfPublishing == year)
                    .Select(b => new BookDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Description = b.Description,
                        YearOfPublishing = b.YearOfPublishing,
                        Quantity = b.Quantity,
                        Genre = b.Genre,
                        AuthorName = b.Author.Name
                    })
                    .ToListAsync();

                return books;
            }
        }
    }
}
