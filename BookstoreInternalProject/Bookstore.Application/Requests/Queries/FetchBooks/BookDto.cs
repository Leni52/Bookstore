﻿using Bookstore.Domain.Common;
using System;

namespace Bookstore.Application.Requests.Queries.FetchBooks
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int YearOfPublishing { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public GenreType Genre { get; set; }
        public string AuthorName { get; set; }
    }
}
