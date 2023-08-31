using Bookstore.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.Commands.CommandsBooks
{
    public class DeleteBookByIdCommand : IRequest<Guid>
    {
        public Guid BookId { get; set; }
        public DeleteBookByIdCommand(Guid bookid)
        {
            BookId = bookid;
        }
        public class DeleteBookByIdHandler : IRequestHandler<DeleteBookByIdCommand, Guid>
        {
            private readonly IBookStoreContext context;
            public DeleteBookByIdHandler(IBookStoreContext context)
            {
                this.context = context;
            }
            public async Task<Guid> Handle(DeleteBookByIdCommand request, CancellationToken cancellationToken)
            {
                var book = await context
                     .Books
                     .Where(b => b.Id == request.BookId)
                     .FirstOrDefaultAsync();
                if (book == null)
                {
                    throw new ArgumentNullException(nameof(book));
                }
                if (book.Quantity > 0)
                {
                    book.Quantity -= 1;
                }
                context.Books.Remove(book);
                await context.SaveChangesAsync();
                return book.Id;
            }
        }
    }
}
