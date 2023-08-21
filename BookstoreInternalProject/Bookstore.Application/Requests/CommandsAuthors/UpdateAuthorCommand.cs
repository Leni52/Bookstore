using Bookstore.Application.Common.Interfaces;
using Bookstore.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.CommandsAuthors
{
    public class UpdateAuthorCommand : IRequest<Author>
    {
        public Guid AuthorId { get; set; }
        public string Title { get; set; }
        public string Biography { get; set; }
        public UpdateAuthorCommand(string title, string biography)
        {
            Title = title;
            Biography = biography;
        }
        public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Author>
        {
            private readonly IBookStoreContext context;
            public UpdateAuthorCommandHandler(IBookStoreContext context)
            {
                this.context = context;
            }

            public async Task<Author> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
            {
                var authorToUpdate = await context.Authors.Where(a => a.Id == command.AuthorId).FirstOrDefaultAsync();
                if (authorToUpdate == null)
                {
                    throw new ArgumentNullException(nameof(authorToUpdate));
                }
                authorToUpdate.Name = command.Title;
                authorToUpdate.Biography = command.Biography;
                authorToUpdate.ModifiedAt = DateTime.Now;
                await context.SaveChangesAsync();
                return authorToUpdate;
            }
        }
    }
}
