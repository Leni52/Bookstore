using Bookstore.Application.Requests.Commands.CommandsCustomers;
using FluentValidation;

namespace Bookstore.Application.Requests.Commands.Validators.ValidatorsCommandsOrders
{
    public class MakeOrderCommandValidator : AbstractValidator<MakeOrderCommand>
    {
        public MakeOrderCommandValidator()
        {
            RuleFor(command => command.CustomerName).NotEmpty().WithMessage("Name is required.");

            RuleFor(command => command.BookQuantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(command => command.CustomerAdress).NotEmpty().WithMessage("Adress is required.");


        }
    }
}
