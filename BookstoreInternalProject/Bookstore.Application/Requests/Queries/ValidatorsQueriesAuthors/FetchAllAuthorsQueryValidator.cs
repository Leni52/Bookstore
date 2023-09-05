using Bookstore.Application.Requests.Queries.FetchAuthors;
using FluentValidation;

namespace Bookstore.Application.Requests.Queries.ValidatorsQueriesAuthors
{
    public class FetchAllAuthorsQueryValidator : AbstractValidator<FetchAllAuthorsQuery>
    {
        public FetchAllAuthorsQueryValidator()
        {

        }
    }
}
