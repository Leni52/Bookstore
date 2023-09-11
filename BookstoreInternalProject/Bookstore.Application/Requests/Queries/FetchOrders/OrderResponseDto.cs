using Bookstore.Domain.Common;
using Bookstore.Domain.Entities;
using System.Collections.Generic;

namespace Bookstore.Application.Requests.Queries.FetchOrders
{
    public class OrderResponseDto
    {
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public double TotalAmount { get; set; }
        public ICollection<Book> OrderedBooks { get; set; }
        public OrderStatus OrderStatus { get; set; }

    }
}
