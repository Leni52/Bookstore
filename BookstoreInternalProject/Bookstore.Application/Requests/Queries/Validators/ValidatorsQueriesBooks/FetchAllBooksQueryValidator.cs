using Bookstore.Application.Requests.Queries.FetchBooks;
using FluentValidation;

namespace Bookstore.Application.Requests.Queries.Validators.ValidatorsQueriesBooks
{
    public class FetchAllBooksQueryValidator : AbstractValidator<FetchAllBooksQuery>
    {
        public FetchAllBooksQueryValidator()
        {

        }
    }
}
