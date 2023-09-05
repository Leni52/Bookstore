using Bookstore.Application.Requests.Queries.FetchBooks;
using FluentValidation;

namespace Bookstore.Application.Requests.Queries.Validators
{
    public class FetchAllBooksQueryValidator : AbstractValidator<FetchAllBooksQuery>
    {
        public FetchAllBooksQueryValidator()
        {

        }
    }
}
