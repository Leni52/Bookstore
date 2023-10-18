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
    public class AddBookCommandHandlerTests
    {
        [Fact]

        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            // Arrange
            var command = new AddBookCommand
            {
                Title = "Sample Book",
                Description = "Sample Description",
                YearOfPublishing = 2023,
                Quantity = 10,
                Price = 19.99,
                Genre = Domain.Common.GenreType.ScienceFiction,
                AuthorName = "Sample Author"
            };

            var authors = new List<Author>
    {
        new Author { Id = Guid.NewGuid(), Name = "Sample Author", Biography = "bio", Books = new List<Book>() }
    };

            var mockAuthorsDbSet = new Mock<DbSet<Author>>();

            var queryableAuthors = authors.AsQueryable();
            mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.Provider).Returns(authors.AsQueryable().Provider);
            mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.Expression).Returns(authors.AsQueryable().Expression);
            mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.ElementType).Returns(authors.AsQueryable().ElementType);
            mockAuthorsDbSet.As<IQueryable<Author>>().Setup(m => m.GetEnumerator()).Returns(() => authors.AsQueryable().GetEnumerator());


            var mockContext = new Mock<IBookStoreContext>();

            // Configure the context to return the DbSet
            mockContext.Setup(c => c.Authors).Returns(mockAuthorsDbSet.Object);

            var mockValidator = new Mock<IValidator<AddBookCommand>>();

            // Mock the validation result to indicate success
            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            // Mock the addition of a new Book
            mockContext.Setup(c => c.Books.Add(It.IsAny<Book>()));
            mockContext.Setup(c => c.SaveChangesAsync(CancellationToken.None))
                .ReturnsAsync(1); // Simulate one record added

            var handler = new AddBookCommand.AddBookCommandHandler(mockContext.Object, mockValidator.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new AddBookCommand
            {
                // Invalid command with missing required properties
            };

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<AddBookCommand>>();

            // Mock the validation result to indicate validation failure
            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure() }));

            var handler = new AddBookCommand.AddBookCommandHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_MissingTitle_ThrowsValidationException()
        {
            // Arrange
            var command = new AddBookCommand
            {
                Title = null, //invalid title
                Description = "Sample Description",
                YearOfPublishing = 2023,
                Quantity = 10,
                Price = 19.99,
                Genre = Domain.Common.GenreType.ScienceFiction,
                AuthorName = "Author1"
            };

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<AddBookCommand>>();

            // Mock the validation result to indicate failure
            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
             .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Title", "Title is required.") }));

            var handler = new AddBookCommand.AddBookCommandHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Contains("Title is required.", exception.Message);
        }

        [Fact]
        public async Task Handle_InvalidYear_ThrowsValidationException()
        {
            // Arrange
            var command = new AddBookCommand
            {
                Title = "Title",
                Description = "Sample Description",
                YearOfPublishing = DateTime.UtcNow.Year + 1,
                Quantity = 10,
                Price = 19.99,
                Genre = Domain.Common.GenreType.ScienceFiction,
                AuthorName = "Author1"
            };

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<AddBookCommand>>();

            // Mock the validation result to indicate failure           
            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("YearOfPublishing", "Invalid year of publishing.") }));

            var handler = new AddBookCommand.AddBookCommandHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Contains("Invalid year of publishing.", exception.Message);


        }
        [Fact]
        public async Task Handle_InvalidPrice_ThrowsValidationException()
        {
            // Arrange
            var command = new AddBookCommand
            {
                Title = "Title1",
                Description = "Sample Description",
                YearOfPublishing = DateTime.UtcNow.Year + 1,
                Quantity = 10,
                Price = -10,
                Genre = Domain.Common.GenreType.ScienceFiction,
                AuthorName = "Author1"
            };

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<AddBookCommand>>();

            // Mock the validation result to indicate failure           
            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Price", "Price must be greater than 0.") }));

            var handler = new AddBookCommand.AddBookCommandHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Contains("Price must be greater than 0.", exception.Message);
        }
    }
}
