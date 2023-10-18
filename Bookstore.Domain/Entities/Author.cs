using System.Collections.Generic;

namespace Bookstore.Domain.Entities
{
    public class Author : Entity
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public string Name { get; set; }
        public string Biography { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
