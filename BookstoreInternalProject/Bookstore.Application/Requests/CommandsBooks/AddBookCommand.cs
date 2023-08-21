using Bookstore.Application.Common.Interfaces;
using Bookstore.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Application.Requests.Commands
{
    public class AddBookCommand : IRequest<bool>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int YearOfPublishing { get; set; }
        public GenreType Genre { get; set; }
        public string AuthorName { get; set; }
        public class AddBookCommandHandler : IRequestHandler<AddBookCommand, bool>
        {
            private readonly IBookStoreContext context;
            public AddBookCommandHandler(IBookStoreContext context)
            {
                this.context = context;
            }

            public async Task<bool> Handle(AddBookCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var author = await this.context.Authors.FirstOrDefaultAsync(a => a.Name == request.AuthorName);
                    if (author == null)
                    {
                        author = new Domain.Entities.Author { Name = request.AuthorName };
                        this.context.Authors.Add(author);
                        await this.context.SaveChangesAsync();
                    }
                    this.context.Books.Add(new Domain.Entities.Book
                    {
                        Title = request.Title,
                        Description = request.Description,
                        Genre = request.Genre,
                        YearOfPublishing = request.YearOfPublishing,
                        AuthorId = author.Id,
                    });
                    await this.context.SaveChangesAsync();
                    return true;
                }

                catch (System.Exception)
                {

                    return false;
                }
            }
        }
    }
}
