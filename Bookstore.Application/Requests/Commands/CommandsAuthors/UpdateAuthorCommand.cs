using Bookstore.Application.Common.Interfaces;
using Bookstore.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.Commands.CommandsAuthors
{
    public class UpdateAuthorCommand : IRequest<Author>
    {
        public Guid AuthorId { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public UpdateAuthorCommand(string name, string biography)
        {
            Name = name;
            Biography = biography;
        }
        public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Author>
        {
            private readonly IBookStoreContext context;
            private readonly IValidator<UpdateAuthorCommand> validator;
            public UpdateAuthorCommandHandler(IBookStoreContext context, IValidator<UpdateAuthorCommand> validator)
            {
                this.context = context;
                this.validator = validator;
            }

            public async Task<Author> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
            {
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var authorToUpdate = await context.Authors.Where(a => a.Id == command.AuthorId).FirstOrDefaultAsync();
                if (authorToUpdate == null)
                {
                    throw new ArgumentNullException(nameof(authorToUpdate));
                }
                authorToUpdate.Name = command.Name;
                authorToUpdate.Biography = command.Biography;
                authorToUpdate.ModifiedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
                return authorToUpdate;
            }
        }
    }
}
