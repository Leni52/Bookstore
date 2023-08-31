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
    public class FetchBooksByAuthorQuery : IRequest<IEnumerable<BookDto>>
    {
        public string AuthorName { get; set; }
        public class FetchBooksByAuthorQueryHandler : IRequestHandler<FetchBooksByAuthorQuery, IEnumerable<BookDto>>
        {
            private readonly IBookStoreContext context;

            public FetchBooksByAuthorQueryHandler(IBookStoreContext context)
            {
                this.context = context;
            }
            public async Task<IEnumerable<BookDto>> Handle(FetchBooksByAuthorQuery request, CancellationToken cancellationToken)
            {

                var authorName = request.AuthorName;

                var books = await this.context
                    .Books
                    .Include(b => b.Author)
                    .Where(b => b.Author.Name == authorName)
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
