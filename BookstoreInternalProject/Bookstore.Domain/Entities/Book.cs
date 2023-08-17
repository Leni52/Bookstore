using System.Net;

namespace Bookstore.Domain.Entities
{
    public class Book : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public GenreType Genre { get; set; }
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
