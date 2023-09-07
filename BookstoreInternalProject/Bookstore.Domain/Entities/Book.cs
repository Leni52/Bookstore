using Bookstore.Domain.Common;
using System;

namespace Bookstore.Domain.Entities
{
    public class Book : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int YearOfPublishing { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public GenreType Genre { get; set; }
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
