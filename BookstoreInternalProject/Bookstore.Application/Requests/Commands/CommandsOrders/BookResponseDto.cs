using Bookstore.Domain.Common;

namespace Bookstore.Application.Requests.Commands.CommandsOrders
{
    public class BookResponseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int YearOfPublishing { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public GenreType Genre { get; set; }
    }
}
