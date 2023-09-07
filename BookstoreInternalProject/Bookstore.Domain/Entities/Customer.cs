using System;
using System.Collections.Generic;

namespace Bookstore.Domain.Entities
{
    public class Customer : Entity
    {
        public Customer()
        {
            Books = new HashSet<Book>();
        }

        public string Name { get; set; }
        public string Adress { get; set; }
        public ICollection<Book> Books { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
