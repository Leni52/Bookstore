using Bookstore.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.Commands.CommandsAuthors
{
    public class AddAuthorCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string Biography { get; set; }
        public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, bool>
        {
            private readonly IBookStoreContext context;
            private readonly IValidator<AddAuthorCommand> validator;
            public AddAuthorCommandHandler(IBookStoreContext context, IValidator<AddAuthorCommand> validator)
            {
                this.context = context;
                this.validator = validator;
            }
            public async Task<bool> Handle(AddAuthorCommand command, CancellationToken cancellationToken)
            {
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                try
                {
                    context.Authors.Add(new Domain.Entities.Author
                    {
                        Name = command.Name,
                        Biography = command.Biography,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow

                    });
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
