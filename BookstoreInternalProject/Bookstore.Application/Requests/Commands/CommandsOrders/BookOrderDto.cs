using System;

namespace Bookstore.Application.Requests.Commands.CommandsOrders
{
    public class BookOrderDto
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
