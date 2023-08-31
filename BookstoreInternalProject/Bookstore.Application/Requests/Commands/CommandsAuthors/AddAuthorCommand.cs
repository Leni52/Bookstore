using Bookstore.Application.Common.Interfaces;
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
            public AddAuthorCommandHandler(IBookStoreContext context)
            {
                this.context = context;
            }
            public async Task<bool> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    context.Authors.Add(new Domain.Entities.Author
                    {
                        Name = request.Name,
                        Biography = request.Biography,
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
