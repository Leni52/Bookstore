using Bookstore.Application.Common.Interfaces;
using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Infrastructure.Persistence
{
    internal class BookstoreContext : DbContext, IBookStoreContext
    {

        public BookstoreContext(DbContextOptions<BookstoreContext> options)
            : base(options)
        {

        }
        public BookstoreContext()
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("ConnectionStrings:Additional");
            }

            base.OnConfiguring(optionsBuilder);
        }
    }


}
