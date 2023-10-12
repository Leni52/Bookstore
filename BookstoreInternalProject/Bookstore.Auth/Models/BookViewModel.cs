namespace Bookstore.Auth.Models
{
    public class BookViewModel
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int YearOfPublishing { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Genre { get; set; }
        public string AuthorName { get; set; }


    }
}
