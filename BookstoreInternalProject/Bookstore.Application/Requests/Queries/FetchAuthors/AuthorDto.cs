using Bookstore.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Bookstore.Application.Requests.Queries.FetchAuthors
{
    public class AuthorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
