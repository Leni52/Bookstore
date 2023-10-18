using Bookstore.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Bookstore.Application.Requests.Queries.FetchAuthors
{
    public class FetchAllAuthorsQuery : IRequest<IEnumerable<AuthorDto>>
    {
        public class FetchAllAuthorsQueryHandler : IRequestHandler<FetchAllAuthorsQuery, IEnumerable<AuthorDto>>
        {
            private readonly IBookStoreContext context;
            private readonly IValidator<FetchAllAuthorsQuery> validator;

            public FetchAllAuthorsQueryHandler(IBookStoreContext context, IValidator<FetchAllAuthorsQuery> validator)
            {
                this.context = context;
                this.validator = validator;
            }

            public async Task<IEnumerable<AuthorDto>> Handle(FetchAllAuthorsQuery request, CancellationToken cancellationToken)
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

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
