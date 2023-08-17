using Bookstore.Domain.Common;
using System;

namespace Bookstore.Application.Requests.Queries.FetchBooks
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public GenreType Genre { get; set; }
        public string AuthorName { get; set; }
    }
}
