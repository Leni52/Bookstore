using Bookstore.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.CommandsBooks
{
    public class DeleteAuthorByIdCommand : IRequest<Guid>
    {
        public Guid BookId { get; set; }
        public DeleteAuthorByIdCommand(Guid bookid)
        {
            BookId = bookid;
        }
        public class DeleteBookByIdHandler : IRequestHandler<DeleteAuthorByIdCommand, Guid>
        {
            private readonly IBookStoreContext context;
            public DeleteBookByIdHandler(IBookStoreContext context)
            {
                this.context = context;
            }
            public async Task<Guid> Handle(DeleteAuthorByIdCommand request, CancellationToken cancellationToken)
            {
                var book = await context
                     .Books
                     .Where(b => b.Id == request.BookId)
                     .FirstOrDefaultAsync();
                if (book == null)
                {
                    throw new ArgumentNullException(nameof(book));
                }
                context.Books.Remove(book);
                await context.SaveChangesAsync();
                return book.Id;
            }
        }
    }
}
