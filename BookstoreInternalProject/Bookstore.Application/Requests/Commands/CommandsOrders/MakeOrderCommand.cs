using AutoMapper;
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
        public ICollection<BookOrderDto> OrderedBooksDto { get; set; }

        public class MakeOrderCommandHandler : IRequestHandler<MakeOrderCommand, bool>
        {
            private readonly IBookStoreContext context;
            private readonly IValidator<MakeOrderCommand> validator;
            private readonly IMapper mapper;

            public MakeOrderCommandHandler(
                IBookStoreContext context,
                IValidator<MakeOrderCommand> validator,
                IMapper mapper
            )
            {
                this.context = context;
                this.validator = validator;
                this.mapper = mapper;
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
                    double totalAmount = CalculateTotalAmount(command.OrderedBooksDto);
                    var orderedBooks = mapper.Map<ICollection<Book>>(command.OrderedBooksDto);

                    var orderedItems = mapper.Map<ICollection<OrderItem>>(command.OrderedBooksDto);

                    var order = new Order
                    {
                        CustomerName = command.CustomerName,
                        CustomerAddress = command.CustomerAdress,
                        TotalAmount = totalAmount,
                        OrderItems = orderedItems,
                        CreatedAt = DateTime.UtcNow,
                        OrderStatus = OrderStatus.Received
                    };

                    foreach (var bookOrderDto in command.OrderedBooksDto)
                    {
                        var book = context.Books.FirstOrDefault(x => x.Title == bookOrderDto.Title);
                        if (book != null)
                        {
                            book.Quantity -= bookOrderDto.Quantity;
                        }

                    }

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
