using Bookstore.Application.Requests.Commands.CommandsAuthors;
using FluentValidation;

namespace Bookstore.Application.Requests.Commands.Validators.ValidatorsCommandsAuthors
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MinimumLength(3)
                .WithMessage("Name must be at least 3 characters.")
                .MaximumLength(50)
                .WithMessage("Name cannot exceed 50 characters.");

            RuleFor(command => command.Biography)
                .NotEmpty()
                .WithMessage("Biography is required.")
                .MinimumLength(3)
                .WithMessage("Biography must be at least 3 characters.")
                .MaximumLength(150)
                .WithMessage("Biography cannot exceed 150 characters.");
        }
    }
}
