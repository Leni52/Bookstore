using Bookstore.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.Queries.FetchAuthors
{
    public class FetchAllAuthorsQuery : IRequest<IEnumerable<AuthorDto>>
    {
        public class FetchAllAuthorsQueryHandler : IRequestHandler<FetchAllAuthorsQuery, IEnumerable<AuthorDto>>
        {
            private readonly IBookStoreContext context;

            public FetchAllAuthorsQueryHandler(IBookStoreContext context)
            {
                this.context = context;
            }

            public async Task<IEnumerable<AuthorDto>> Handle(FetchAllAuthorsQuery request, CancellationToken cancellationToken)
            {
                var authors = await this.context
                    .Authors
                    .Include(a => a.Books)
                    .Select(a => new AuthorDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Biography = a.Biography,
                        Books = a.Books,
                    })
                    .ToListAsync();

                return authors;
            }
        }
    }

}
