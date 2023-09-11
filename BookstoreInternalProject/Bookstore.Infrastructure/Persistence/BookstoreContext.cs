using Bookstore.Application.Common.Interfaces;
using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Infrastructure.Persistence
{
    internal class BookstoreContext : DbContext, IBookStoreContext
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options)
            : base(options) { }

        public BookstoreContext() { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("ConnectionStrings:Default");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Customer>()
                .HasOne(customer => customer.Order)
                .WithOne(order => order.Customer)
                .HasForeignKey<Order>(order => order.CustomerId);

            modelBuilder
                .Entity<Author>()
                .HasMany(author => author.Books)
                .WithOne(book => book.Author)
                .HasForeignKey(book => book.AuthorId);

            modelBuilder
                .Entity<Order>()
                .HasOne(order => order.Customer)
                .WithOne(customer => customer.Order)
                .HasForeignKey<Customer>(customer => customer.OrderId);

            modelBuilder.Entity<OrderItem>().HasKey(oi => new { oi.OrderId, oi.BookId });

            modelBuilder
                .Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(order => order.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Book)
                .WithMany(b => b.OrderItems)
                .HasForeignKey(oi => oi.BookId);
        }
    }
}
