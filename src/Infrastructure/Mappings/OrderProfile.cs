using AutoMapper;
using Infrastructure.MediatR.Orders.Commands;
using Infrastructure.Models.Orders;

namespace Infrastructure.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CreateOrderRequest, Order>();
        CreateMap<CreateOrderItem, Order.OrderItem>();
    }
}