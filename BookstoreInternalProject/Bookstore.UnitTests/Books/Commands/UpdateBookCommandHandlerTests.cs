using Bookstore.Application.Common.Interfaces;
using Bookstore.Application.Requests.Commands.CommandsBooks;
using Bookstore.Domain.Common;
using Bookstore.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Bookstore.UnitTests.Books.Commands
{
    public class UpdateBookCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_UpdatesBook()
        {
            //arrange
            var bookId = Guid.NewGuid();
            var authorId = Guid.NewGuid();
            var command = new UpdateBookCommand(
                bookId,
                "Updated Title",
                "Updated description",
                2023,
                20,
                15,
                authorId,
                Domain.Common.GenreType.ScienceFiction);
            var author = new Author
            {
                Id = authorId,
                Name = "Author1",
                Biography = "Biography1",
                Books = new List<Book>()
            };
            var books = new List<Book>
            {
                new Book
                {
                    Id = bookId,
                    Title = "Original title",
                    Description = "Original description",
                    YearOfPublishing = 2021,
                    Price = 10,
                    Quantity = 5,
                    AuthorId = authorId,
                    Genre = Domain.Common.GenreType.Technical
                }
            };

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<UpdateBookCommand>>();

            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            var mockDbSet = new Mock<DbSet<Book>>();
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(books.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(books.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(books.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(() => books.AsQueryable().GetEnumerator());


            mockContext.Setup(c => c.Books).Returns(mockDbSet.Object);

            var handler = new UpdateBookCommand.UpdateBookCommandHandler(mockContext.Object, mockValidator.Object);

            //act
            var updatedBook = await handler.Handle(command, CancellationToken.None);

            //assert
            Assert.NotNull(updatedBook);
            Assert.Equal(command.Title, updatedBook.Title);
            Assert.Equal(command.Description, updatedBook.Description);
            Assert.Equal(command.YearOfPublishing, updatedBook.YearOfPublishing);
            Assert.Equal(command.Price, updatedBook.Price);
            Assert.Equal(command.Quantity, updatedBook.Quantity);
            Assert.Equal(command.AuthorId, updatedBook.AuthorId);
            Assert.Equal(command.Genre, updatedBook.Genre);
            mockContext.Verify(c => c.SaveChangesAsync(CancellationToken.None), Times.Once);

        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new UpdateBookCommand(bookId, null, null, 0, 0, 0, Guid.Empty, 0);

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<UpdateBookCommand>>();

            // Mock the validation result to indicate validation failure
            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("PropertyName", "Error Message") }));

            var handler = new UpdateBookCommand.UpdateBookCommandHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_BookNotFound_ThrowsArgumentNullException()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new UpdateBookCommand(
                bookId,
                "Updated Title",
                "Updated Description",
                2023,
                29.99,
                15,
                Guid.NewGuid(),
                GenreType.ScienceFiction
            );

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<UpdateBookCommand>>();

            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            var handler = new UpdateBookCommand.UpdateBookCommandHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));

        }

        [Fact]
        public async Task Handle_InvalidCommandMissingTitle_ThrowsValidationException()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new UpdateBookCommand(
                bookId,
                title: null, // Invalid, title is required
                description: "Updated Description",
                yearOfPublishing: 2023,
                price: 29.99,
                quantity: 15,
                authorId: Guid.NewGuid(),
                genre: GenreType.ScienceFiction
            );

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<UpdateBookCommand>>();

            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Title", "Title is required.") }));

            var handler = new UpdateBookCommand.UpdateBookCommandHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvalidDescription_ThrowsValidationException()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new UpdateBookCommand(
                bookId,
                title: "New Title",
                description: "", //must be at least 3 characters
                yearOfPublishing: 2023,
                price: 29.99,
                quantity: 15,
                authorId: Guid.NewGuid(),
                genre: GenreType.ScienceFiction
            );

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<UpdateBookCommand>>();

            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Description", "Description must be at least 3 characters.") }));

            var handler = new UpdateBookCommand.UpdateBookCommandHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvalidYearOfPublishing_ThrowsValidationException()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new UpdateBookCommand(
                bookId,
                "Updated Title",
                "Updated Description",
                yearOfPublishing: 1799, // Invalid year
                price: 20,
                quantity: 15,
                authorId: Guid.NewGuid(),
                genre: GenreType.ScienceFiction
            );

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<UpdateBookCommand>>();

            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("YearOfPublishing", "Invalid year of publishing.") }));

            var handler = new UpdateBookCommand.UpdateBookCommandHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvalidPrice_ThrowsValidationException()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new UpdateBookCommand(
                bookId,
                "Updated Title",
                "Updated Description",
                yearOfPublishing: 2023,
                price: -5, // Invalid price
                quantity: 15,
                authorId: Guid.NewGuid(),
                genre: GenreType.ScienceFiction
            );

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<UpdateBookCommand>>();

            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Price", "Price must be greater than 0.") }));

            var handler = new UpdateBookCommand.UpdateBookCommandHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvalidQuantity_ThrowsValidationException()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new UpdateBookCommand(
                bookId,
                "Updated Title",
                "Updated Description",
                yearOfPublishing: 2023,
                price: 29.99,
                quantity: -5, // Invalid quantity
                authorId: Guid.NewGuid(),
                genre: GenreType.ScienceFiction
            );

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<UpdateBookCommand>>();

            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Quantity", "Quantity must be greater than 0.") }));

            var handler = new UpdateBookCommand.UpdateBookCommandHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvalidGenre_ThrowsValidationException()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new UpdateBookCommand(
                bookId,
                "Updated Title",
                "Updated Description",
                yearOfPublishing: 2023,
                price: 29.99,
                quantity: 5,
                authorId: Guid.NewGuid(),
                genre: (GenreType)1000 // An invalid genre value
            );

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<UpdateBookCommand>>();

            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Genre", "Invalid genre.") }));

            var handler = new UpdateBookCommand.UpdateBookCommandHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

    }
}

