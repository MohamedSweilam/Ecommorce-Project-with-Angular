using AutoMapper;
using Ecommorce.API.Helper;
using Ecommorce.Core.DTO;
using Ecommorce.Core.interfaces;
using Ecommorce.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Net.WebSockets;

namespace Ecommorce.API.Controllers
{

    public class ProductsController : BaseController
    {
        public ProductsController(IUnitofwork unitwork, IMapper mapper) : base(unitwork, mapper)
        {
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> Getall([FromQuery]ProductParams productParams)
        {
            try
            {
            var product = await _unitwork.ProductRepository.GetAllAsync(productParams);
            

                return Ok(new Pagination<ProductDTO>(productParams.PageNumber,productParams.pageSize,product.TotalCount,product.products));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _unitwork.ProductRepository.GetByIdAsync(id,x=>x.Category,x=>x.Photos);
                var result = _mapper.Map<ProductDTO>(product);
                if (product is null) return BadRequest(new ApiResponse(400));
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Add-Product")]
        public async Task<IActionResult> AddProduct(AddProductDTO productDTO)
        {
            try
            {
                await _unitwork.ProductRepository.AddAsync(productDTO);
                return Ok(new ApiResponse(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse(400,ex.Message));
            }
        }
        [HttpPut("Update-Product")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO updateProductDTO)
        {
            try
            {
                await _unitwork.ProductRepository.UpdateAsync(updateProductDTO);
                return Ok(new ApiResponse(200));

            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }
        [HttpDelete("Delete-Product/{Id}")]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            try
            {
                var product =await _unitwork.ProductRepository.GetByIdAsync(Id,m=>m.Photos,m=>m.Category);

                
                await _unitwork.ProductRepository.DeleteAsync(product);

                return Ok(new ApiResponse(200));


            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse(400, ex.Message)); 
            }
        }
    }
}
