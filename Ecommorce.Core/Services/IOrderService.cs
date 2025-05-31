using Ecommorce.Core.DTO;
using Ecommorce.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce.Core.Services
{
    public interface IOrderService
    {
         Task<Orders> CreateOrderAsync(OrderDTO orderDTO , string BuyerEmail);
         Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail);
         Task<OrderToReturnDTO> GetOrdersByIdAsync(int Id,string BuyerEmail);
         Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();




    }
}
