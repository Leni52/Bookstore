using Bookstore.Application.Common.Interfaces;
using Bookstore.Application.Requests.Commands.CommandsOrders;
using Bookstore.Domain.Common;
using Bookstore.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.Commands.CommandsCustomers
{
    public class MakeOrderCommand : IRequest<bool>
    {
        public string CustomerName { get; set; }
        public string CustomerAdress { get; set; }
        public int BookQuantity { get; set; }
        public ICollection<BookOrderDto> OrderedBooks { get; set; }

        public class MakeOrderCommandHandler : IRequestHandler<MakeOrderCommand, bool>
        {
            private readonly IBookStoreContext context;
            private readonly IValidator<MakeOrderCommand> validator;

            public MakeOrderCommandHandler(
                IBookStoreContext context,
                IValidator<MakeOrderCommand> validator
            )
            {
                this.context = context;
                this.validator = validator;
            }

            public async Task<bool> Handle(
                MakeOrderCommand command,
                CancellationToken cancellationToken
            )
            {
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                try
                {
                    double totalAmount = CalculateTotalAmount(command.OrderedBooks);
                    var order = new Order
                    {
                        CustomerName = command.CustomerName,
                        CustomerAddress = command.CustomerAdress,
                        TotalAmount = totalAmount,
                        // OrderedBooks = command.OrderedBooks,
                        CreatedAt = DateTime.UtcNow,
                        OrderStatus = OrderStatus.Received
                    };

                    context.Orders.Add(order);
                    await context.SaveChangesAsync();

                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            private double CalculateTotalAmount(ICollection<BookOrderDto> orderedBooks)
            {
                double totalAmount = 0;

                foreach (var bookOrderDto in orderedBooks)
                {
                    double bookPrice = GetBookPriceByTitle(bookOrderDto.Title);

                    double subtotal = bookPrice * bookOrderDto.Quantity;

                    totalAmount += subtotal;
                }

                return totalAmount;
            }

            private double GetBookPriceByTitle(string bookTitle)
            {
                var book = context.Books.FirstOrDefault(x => x.Title == bookTitle);
                if (book != null)
                {
                    return book.Price;
                }
                return 0;
            }
        }
    }
}
