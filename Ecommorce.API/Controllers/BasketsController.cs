using AutoMapper;
using Ecommorce.API.Helper;
using Ecommorce.Core.Entities;
using Ecommorce.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ecommorce.API.Controllers
{
    
    public class BasketsController : BaseController
    {
        public BasketsController(IUnitofwork _unitwork, IMapper _mapper) : base(_unitwork, _mapper)
        {
        }
        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult>GetBasketItemById(string id)
        {
            var result = await _unitwork.CustomerBasket.GetBasketAsync(id);
            if(result is null)
            {
                return Ok(new CustomerBasket());
            }
            return Ok(result);
        }
        [HttpPost("add-basket-item")]
        public async Task<IActionResult> AddBasektItem(CustomerBasket basket)
        {
            var _basket = await _unitwork.CustomerBasket.UpdateBasketAsync(basket);
            if (_basket is not null)
            {
                return Ok(_basket);
            }
            return BadRequest(new ApiResponse(400));

        }
        [HttpDelete("delete-item/{id}")]
        public async Task<IActionResult>DeleteBasketItem(string id)
        {
            var result = await _unitwork.CustomerBasket.DeleteBasketAsync(id);
            return result ? Ok(new ApiResponse(200, "Item has been deleted")) : BadRequest(new ApiResponse(400));
        }
    }
}
