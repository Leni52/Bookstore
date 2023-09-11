using System;

namespace Bookstore.Domain.Entities
{
    public class OrderItem
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public int Quantity { get; set; }
    }

}
