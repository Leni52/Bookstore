using Bookstore.Application.Common.Interfaces;
using Bookstore.Application.Requests.Commands.CommandsAuthors;
using Bookstore.Application.Requests.Commands.CommandsBooks;
using Bookstore.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace Bookstore.UnitTests.Authors.Commands
{
    public class AddAuthorCommandTests
    {
        [Fact]
        public async Task Handle_ValidAuthor_AddsAuthorToDatabase()
        {
            // Arrange
            var authorName = "John Doe";
            var authorBiography = "Biography text";

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<AddAuthorCommand>>();

            // Mock the validator to return a valid result
            mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<AddAuthorCommand>(), CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            // Create an instance of the command and handler
            var command = new AddAuthorCommand { Name = authorName, Biography = authorBiography };
            var handler = new AddAuthorCommand.AddAuthorCommandHandler(mockContext.Object, mockValidator.Object);
            // Mock the addition of a new author
            mockContext.Setup(c => c.Authors.Add(It.IsAny<Author>()));
            mockContext.Setup(c => c.SaveChangesAsync(CancellationToken.None))
                .ReturnsAsync(1); // Simulate one record added

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mockContext.Verify(dbContext =>
                dbContext.Authors.Add(It.IsAny<Author>()), Times.Once);

            Assert.True(result);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new AddAuthorCommand
            {
                // Invalid command with missing required properties
            };

            var mockContext = new Mock<IBookStoreContext>();
            var mockValidator = new Mock<IValidator<AddAuthorCommand>>();

            // Mock the validation result to indicate validation failure
            mockValidator.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure() }));

            var handler = new AddAuthorCommand.AddAuthorCommandHandler(mockContext.Object, mockValidator.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
