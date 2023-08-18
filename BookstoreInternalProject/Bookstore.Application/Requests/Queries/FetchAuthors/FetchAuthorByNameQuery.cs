using Bookstore.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.Queries.FetchAuthors
{
    public class FetchAuthorByNameQuery : IRequest<AuthorDto>
    {
        public string AuthorName { get; set; }
        public class FetchAuthorByNameQueryHandler : IRequestHandler<FetchAuthorByNameQuery, AuthorDto>
        {
            private readonly IBookStoreContext context;
            public FetchAuthorByNameQueryHandler(IBookStoreContext context)
            {
                this.context = context;
            }

            public async Task<AuthorDto> Handle(FetchAuthorByNameQuery request, CancellationToken cancellationToken)
            {
                var authorName = request.AuthorName;

                var author = await this.context
                    .Authors
                    .Where(a => a.Name == authorName)
                    .Select(a => new AuthorDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Biography = a.Biography,
                        Books = a.Books
                    })
                    .FirstOrDefaultAsync();

                return author;
            }
        }
    }
}
