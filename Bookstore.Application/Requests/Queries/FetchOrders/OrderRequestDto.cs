using Bookstore.Domain.Entities;
using System.Collections.Generic;

namespace Bookstore.Application.Requests.Queries.FetchOrders
{
    public class OrderRequestDto
    {
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public ICollection<Book> OrderedBooks { get; set; }

    }
}
