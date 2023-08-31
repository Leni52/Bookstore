using Bookstore.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.Queries.FetchBooks
{
    public class FetchBookByIdQuery : IRequest<BookDto>
    {
        public Guid BookId { get; set; }
        public class FetchBookByIdQueryHandler : IRequestHandler<FetchBookByIdQuery, BookDto>
        {
            private readonly IBookStoreContext context;

            public FetchBookByIdQueryHandler(IBookStoreContext context)
            {
                this.context = context;
            }
            public async Task<BookDto> Handle(FetchBookByIdQuery request, CancellationToken cancellationToken)
            {

                var bookId = request.BookId;

                var book = await this.context
                    .Books
                    .Include(b => b.Author)
                    .Where(b => b.Id == bookId)
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
                    .FirstOrDefaultAsync();

                return book;
            }

        }
    }

}

