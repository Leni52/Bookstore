using Bookstore.Application.Common.Interfaces;
using FluentValidation;
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
            private readonly IValidator<FetchAllBooksQuery> validator;

            public FetchAllBooksQueryHandler(IBookStoreContext context, IValidator<FetchAllBooksQuery> validator)
            {
                this.context = context;
                this.validator = validator;
            }

            public async Task<IEnumerable<BookDto>> Handle(FetchAllBooksQuery request, CancellationToken cancellationToken)
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
                var books = await this.context
                    .Books
                    .Include(b => b.Author)
                    .Select(b => new BookDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Description = b.Description,
                        YearOfPublishing = b.YearOfPublishing,
                        Price = b.Price,
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
