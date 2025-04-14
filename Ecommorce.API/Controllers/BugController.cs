using AutoMapper;
using Ecommorce.Core.Entities.Product;
using Ecommorce.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommorce.API.Controllers
{
   
    public class BugController : BaseController
    {
        public BugController(IUnitofwork unitwork, IMapper mapper) : base(unitwork, mapper)
        {
        }
        [HttpGet("not-found")]
        public async Task<ActionResult> GetNotFound()
        {
            var category = await _unitwork.CategoryRepository.GetByIdAsync(100);
            if (category == null) return NotFound();
            return Ok(category);
        }
        [HttpGet("server-error")]
        public async Task<ActionResult> GetServerError()
        {
            var category = await _unitwork.CategoryRepository.GetByIdAsync(100);
            category.Name = "";
            return Ok(category);
        }
        [HttpGet("Bad-Request/{id}")]
        public async Task<ActionResult> GetBadRequest(int id)
        {
            
            return Ok();
        }
        [HttpGet("Bad-Request")]
        public async Task<ActionResult> GetBadRequest()
        {

            return BadRequest();
        }
    }
}
