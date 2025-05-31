using AutoMapper;
using Ecommorce.Core.DTO;
using Ecommorce.Core.Entities.AppUser;
using Ecommorce.Core.Entities.Order;

namespace Ecommorce.API.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<Orders, OrderToReturnDTO>()
                .ForMember(d=>d.deliveryMethod,o=>o.MapFrom(s=>s.deliveryMethod.Name))
                .ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<ShippingAddress, shipAddressDTO>().ReverseMap();
            CreateMap<Address, shipAddressDTO>().ReverseMap();
        }
    }
}
