using Bookstore.Application.Requests.Commands.CommandsBooks;
using FluentValidation;

namespace Bookstore.Application.Requests.Commands.ValidatorsBooks
{
    public class DeleteBookCommandValidator : AbstractValidator<DeleteBookByIdCommand>
    {
        public DeleteBookCommandValidator()
        {
            RuleFor(command => command.BookId).NotEmpty().WithMessage("Book Id is required.");
        }
    }
}
