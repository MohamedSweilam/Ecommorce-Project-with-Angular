using Ecommorce.Core.DTO;
using Ecommorce.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommorce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize]
        [HttpPost("create-orders")]
        public async Task<IActionResult> CreateOrders(OrderDTO orderDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _orderService.CreateOrderAsync(orderDTO, email);
            return Ok(order);
        }
        [HttpGet("get-orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var orders = await _orderService.GetAllOrdersForUserAsync(email);
            return Ok(orders);
        }
        [HttpGet("get-order-by-id/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _orderService.GetOrdersByIdAsync(id,email);
            return Ok(order);
        }
        [Authorize]
        [HttpGet("get-delivery")]
        public async Task<IActionResult> GetDelivery()
            =>Ok(await _orderService.GetDeliveryMethodAsync());



    }
}
