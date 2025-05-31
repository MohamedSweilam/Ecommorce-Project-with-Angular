using AutoMapper;
using Ecommorce.Core.DTO;
using Ecommorce.Core.Entities.Order;
using Ecommorce.Core.interfaces;
using Ecommorce.Core.Services;
using Ecommorce_.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce_.infrastructure.Repositories.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitofwork _unitofwork;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public OrderService(IUnitofwork unitofwork, ApplicationDbContext context, IMapper mapper)
        {
            _unitofwork = unitofwork;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Orders> CreateOrderAsync(OrderDTO orderDTO, string BuyerEmail)
        {
            var basket = await _unitofwork.CustomerBasket.GetBasketAsync(orderDTO.basketId);
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.basketItems)
            {
                var Product = await _unitofwork.ProductRepository.GetByIdAsync(item.Id);
                var orderItem = new OrderItem
                    (Product.Id, Product.Name, item.Image, item.Price, item.Quantity);
                orderItems.Add(orderItem);
            }
            var deliveryMethod = await _context.DeliveryMethods.FirstOrDefaultAsync(m => m.Id == orderDTO.deliveryMethodId);
            var subTotal = orderItems.Sum(m => m.Price * m.Quantity);
            var ship =  _mapper.Map<ShippingAddress>(orderDTO.shipAdress);
            var order = new Orders(BuyerEmail, subTotal, ship, deliveryMethod, orderItems);
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;

        }

        public async Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            var orders = await _context.Orders.Where(m => m.BuyerEmail == BuyerEmail)
                 .Include(inc => inc.orderItems)
                 .Include(inc => inc.deliveryMethod).ToListAsync();
            var result = _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);

            return result;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        => await _context.DeliveryMethods.AsNoTracking().ToListAsync();

        public async Task<OrderToReturnDTO> GetOrdersByIdAsync(int Id, string BuyerEmail)
        {
            var order = await _context.Orders.Where(m => m.Id == Id && m.BuyerEmail == BuyerEmail)
                .Include(inc => inc.orderItems)
                .Include(inc => inc.deliveryMethod).FirstOrDefaultAsync();
            var result = _mapper.Map<OrderToReturnDTO>(order);
            return result;
        }
    }
}
