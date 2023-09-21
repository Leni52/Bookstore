using Bookstore.Application.Common.Interfaces;
using Bookstore.Application.Requests.Commands.CommandsBooks;
using Bookstore.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Bookstore.UnitTests.Books.Commands
{
    public class DeleteBookByIdCommandTests
    {
        [Fact]
        public async Task Handle_ValidCommand_DeletesBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new DeleteBookByIdCommand(bookId);

            var books = new List<Book>
    {
        new Book
        {
            Id = bookId,
            Quantity = 1
        }
    };

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<DeleteBookByIdCommand>>();

            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());


            var mockBooksDbSet = new Mock<DbSet<Book>>();
            var queryableBooks = books.AsQueryable();

            mockBooksDbSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(queryableBooks.Provider);
            mockBooksDbSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(queryableBooks.Expression);
            mockBooksDbSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(queryableBooks.ElementType);
            mockBooksDbSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(queryableBooks.GetEnumerator());

            mockContext.Setup(c => c.Books).Returns(mockBooksDbSet.Object);

            var handler = new DeleteBookByIdCommand.DeleteBookByIdHandler(mockContext.Object, mockValidator.Object);

            // Act
            var deletedBookId = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(bookId, deletedBookId);
            Assert.Equal(0, books[0].Quantity); // Verify that the book quantity was decremented
            mockContext.Verify(c => c.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_BookNotFound_ThrowsArgumentNullException()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new DeleteBookByIdCommand(bookId);

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<DeleteBookByIdCommand>>();

            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());


            var handler = new DeleteBookByIdCommand.DeleteBookByIdHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_BookQuantityGreaterThanZero_DecreasesQuantity()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new DeleteBookByIdCommand(bookId);

            var books = new List<Book>
    {
        new Book
        {
            Id = bookId,
            Quantity = 5
        }
    };

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<DeleteBookByIdCommand>>();

            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());


            var mockBooksDbSet = new Mock<DbSet<Book>>();
            var queryableBooks = books.AsQueryable();

            mockBooksDbSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(queryableBooks.Provider);
            mockBooksDbSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(queryableBooks.Expression);
            mockBooksDbSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(queryableBooks.ElementType);
            mockBooksDbSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(queryableBooks.GetEnumerator());

            mockContext.Setup(c => c.Books).Returns(mockBooksDbSet.Object);

            var handler = new DeleteBookByIdCommand.DeleteBookByIdHandler(mockContext.Object, mockValidator.Object);

            // Act
            var deletedBookId = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(bookId, deletedBookId);
            Assert.Equal(4, books[0].Quantity); // Verify that the book quantity was decremented
            mockContext.Verify(c => c.SaveChangesAsync(CancellationToken.None), Times.Once);


        }
    }
}


