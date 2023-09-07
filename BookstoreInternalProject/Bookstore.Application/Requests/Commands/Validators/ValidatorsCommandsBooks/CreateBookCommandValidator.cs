using Bookstore.Application.Requests.Commands.CommandsBooks;
using FluentValidation;
using System;

namespace Bookstore.Application.Requests.Commands.Validators.ValidatorsCommandsBooks
{
    public class CreateBookCommandValidator : AbstractValidator<AddBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(command => command.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MinimumLength(3)
                .WithMessage("Title must be at least 3 characters.")
                .MaximumLength(50)
                .WithMessage("Title cannot exceed 50 characters.");

            RuleFor(command => command.AuthorName)
                .NotEmpty()
                .WithMessage("Author name is required.");

            RuleFor(command => command.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MinimumLength(3)
                .WithMessage("Description must be at least 3 characters.")
                .MaximumLength(150)
                .WithMessage("Description cannot exceed 150 characters.");

            RuleFor(command => command.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(command => command.Price)
               .GreaterThan(0)
               .WithMessage("Price must be greater than 0.");

            RuleFor(command => command.YearOfPublishing)
                .InclusiveBetween(1900, DateTime.Now.Year)
                .WithMessage("Invalid year of publishing.");

            RuleFor(command => command.Genre).IsInEnum().WithMessage("Invalid genre.");
        }
    }
}
