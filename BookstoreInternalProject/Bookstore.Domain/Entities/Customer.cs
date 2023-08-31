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
        public ICollection<Book> Books { get; set; }
    }
}
