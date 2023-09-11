using AutoMapper;
using Bookstore.Application.Requests.Queries.FetchOrders;
using Bookstore.Domain.Entities;

namespace Bookstore.Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {

            CreateMap<OrderRequestDto, Order>().ReverseMap();
            CreateMap<Order, OrderResponseDto>().ReverseMap();
        }
    }
}
