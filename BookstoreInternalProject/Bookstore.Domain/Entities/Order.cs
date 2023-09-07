using Bookstore.Domain.Common;
using System;
using System.Collections.Generic;

namespace Bookstore.Domain.Entities
{
    public class Order : Entity
    {
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public double TotalAmount { get; set; }
        public ICollection<Book> OrderedBooks { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
