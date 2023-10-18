using Bookstore.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Bookstore.Application.Requests.Commands.CommandsOrders;

namespace Bookstore.Application.Requests.Queries.FetchOrders
{
    public class FetchAllOrdersQuery : IRequest<IEnumerable<OrderResponseDto>>
    {
        public class FetchAllOrdersQueryHandler : IRequestHandler<FetchAllOrdersQuery, IEnumerable<OrderResponseDto>>
        {
            private readonly IBookStoreContext context;
            private readonly IValidator<FetchAllOrdersQuery> validator;

            public FetchAllOrdersQueryHandler(IBookStoreContext context, IValidator<FetchAllOrdersQuery> validator)
            {
                this.context = context;
                this.validator = validator;
            }

            public async Task<IEnumerable<OrderResponseDto>> Handle(FetchAllOrdersQuery request, CancellationToken cancellationToken)
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
                var orders = await this.context
                    .Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                    .Select(r => new OrderResponseDto
                    {
                        CustomerAddress = r.CustomerAddress,
                        CustomerName = r.CustomerName,
                        OrderStatus = r.OrderStatus,
                        OrderedBooks = r.OrderItems.Select(oi => oi.Book).ToList(),
                        TotalAmount = r.OrderItems.Sum(oi => oi.Book.Price * oi.Quantity)
                    })
                    .ToListAsync();

                return orders;
            }

        }
    }
}
