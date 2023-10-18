using Bookstore.Application.Common.Interfaces;
using Bookstore.Application.Requests.Commands.CommandsCustomers;
using FluentValidation;
using System.Linq;

namespace Bookstore.Application.Requests.Commands.Validators.ValidatorsCommandsOrders
{
    public class MakeOrderCommandValidator : AbstractValidator<MakeOrderCommand>
    {
        private readonly IBookStoreContext context;
        public MakeOrderCommandValidator(IBookStoreContext context)
        {
            this.context = context;

            RuleFor(command => command.CustomerName).NotEmpty().WithMessage("Customer Name is required.");

            RuleFor(command => command.BookQuantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(command => command.CustomerAdress).NotEmpty().WithMessage("Adress is required.");

            RuleForEach(command => command.OrderedBooksDto)
                .NotEmpty().WithMessage("At least one book must be ordered.")
                .ChildRules(bookOrderDto =>
                {
                    bookOrderDto.RuleFor(dto => dto.Title).NotEmpty().WithMessage("Book title is required.");
                    bookOrderDto.RuleFor(dto => dto.Quantity)
                   .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
                   .Must((command, quantity, property) => HaveEnoughBooksInStock(command.Title, quantity))
                   .WithMessage("Not enough books in stock.");

                });
        }
        private bool HaveEnoughBooksInStock(string bookTitle, int quantity)
        {
            var book = context.Books.FirstOrDefault(x => x.Title == bookTitle);
            return book != null && book.Quantity >= quantity;
        }

    }
}

