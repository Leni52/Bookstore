using AutoMapper;
using Bookstore.Application.Requests.Commands.CommandsOrders;
using Bookstore.Domain.Entities;

namespace Bookstore.Application.Mappings
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookOrderDto, Book>();
            CreateMap<BookOrderDto, OrderItem>();
            CreateMap<Book, OrderItem>();
            CreateMap<Book, BookResponseDto>();
        }
    }
}
